using Asp.Versioning;

namespace Playwright.Training.Api.Extensions;

/// <summary>
/// Provides extension methods for configuring API versioning services.
/// </summary>
public static class ApiVersioningExtensions
{
    /// <summary>
    /// Configures and adds API versioning and API explorer services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance so that multiple calls can be chained.</returns>
    /// <remarks>
    /// <para>This method sets up:</para>
    /// <list type="bullet">
    /// <item>API versioning using the URL segment (<c>/v{version}/...</c>) to read the API version.</item>
    /// <item>API Explorer with a group format of <c>'v'VVV</c> (e.g., v1.0).</item>
    /// <item>Substitution of the API version in the URL for route generation.</item>
    /// </list>
    /// </remarks>
    public static IServiceCollection ConfigureApiVersioning(this IServiceCollection services)
    {
        services
            .AddApiVersioning(options =>
            {
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
                options.ReportApiVersions = true;
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        return services;
    }
}