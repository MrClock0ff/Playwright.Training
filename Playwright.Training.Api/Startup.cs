using FluentValidation;
using IL.FluentValidation.Extensions.Options;
using Playwright.Training.Api.Extensions;
using Playwright.Training.Api.Middleware;
using Playwright.Training.Api.Services;
using Playwright.Training.Api.Settings;
using Playwright.Training.Api.Validators;
using Playwright.Training.Domain.DTOs;
using Playwright.Training.Domain.Validators;
using Playwright.Training.Infrastructure;
using Playwright.Training.Infrastructure.Repositories;

namespace Playwright.Training.Api;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Adds services to the container.
    /// </summary>
    /// <param name="services">Service collection instance.</param>
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddControllers(options => { options.InsertJsonPatchInputFormatter(0); })
            .AddNewtonsoftJson();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.ConfigureApiVersioning();
        services.AddRouting(options => { options.LowercaseUrls = true; });

        // Authentication
        services
            .AddOptions<KeycloakSettings>()
            .Bind(_configuration.GetSection("Keycloak"))
            .Validate<KeycloakSettings, KeycloakSettingsValidator>()
            .ValidateOnStart();
        services.AddKeycloakAuthentication(_configuration);

        // Database
        services.AddTrainingDatabaseContext(_configuration.GetConnectionString("TrainingDatabase") ??
                                            throw new Exception("TrainingDatabase connection string is null"));

        // Validators
        services.AddTransient<IValidator<CreateUserDto>, CreateUserDtoValidator>();
        services.AddTransient<CreateUserDtoValidator>();
        services.AddTransient<IValidator<Guid>, GuidIdValidator>();
        services.AddTransient<GuidIdValidator>();
        services.AddTransient<IValidator<KeycloakSettings>, KeycloakSettingsValidator>();
        services.AddTransient<KeycloakSettingsValidator>();

        // Services
        services.AddScoped<IUserService, UserService>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
    }

    /// <summary>
    /// Configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app">Application builder instance.</param>
    /// <param name="env">Web host environment instance.</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(builder => { builder.MapControllers(); });
    }
}