using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Playwright.Training.Api.ClaimsTransformations;
using Playwright.Training.Api.Settings;

namespace Playwright.Training.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKeycloakAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorization();
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.IncludeErrorDetails = true;
                options.Authority = configuration["Keycloak:Authority"]
                                    ?? throw new ArgumentNullException(
                                        nameof(KeycloakSettings.Authority),
                                        "Keycloak:Authority cannot be null or empty.");
                options.Audience = configuration["Keycloak:Audience"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuers = [configuration["Keycloak:ValidIssuer"]],
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false, // Must be true for production
                    ValidateLifetime = true,
                    ValidateIssuer = true
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"{nameof(JwtBearerEvents.OnAuthenticationFailed)}: exception message={context.Exception.Message}");
                        Console.WriteLine($"{nameof(JwtBearerEvents.OnAuthenticationFailed)}: inner exception message={context.Exception.InnerException?.Message}");

                        foreach (string key in context.Exception.Data.Keys)
                        {
                            Console.WriteLine($"{nameof(JwtBearerEvents.OnAuthenticationFailed)}: {key}={context.Exception.Data[key]}");    
                        }
                        
                        return Task.CompletedTask;
                    }
                };
                
                options.BackchannelHttpHandler = new BackchannelHttpHandler();
                options.BackchannelTimeout = TimeSpan.FromSeconds(30);
            });
        services.AddTransient<IClaimsTransformation, RoleClaimsTransformation>();

        return services;
    }
    
    private class BackchannelHttpHandler : DelegatingHandler
    {
        public BackchannelHttpHandler() : base(new HttpClientHandler())
        {
        }
        
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {

                Console.WriteLine($"{nameof(BackchannelHttpHandler)}.{nameof(SendAsync)}: request URL={request.RequestUri?.AbsoluteUri}");
                HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync(cancellationToken);
                    Console.WriteLine($"{nameof(BackchannelHttpHandler)}.{nameof(SendAsync)}: request content={content}");
                }
                else
                {
                    Console.WriteLine($"{nameof(BackchannelHttpHandler)}.{nameof(SendAsync)}: response status code={response.StatusCode}");
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{nameof(BackchannelHttpHandler)}.{nameof(SendAsync)} exception message={e.Message}");
                throw;
            }
        }
    }
}