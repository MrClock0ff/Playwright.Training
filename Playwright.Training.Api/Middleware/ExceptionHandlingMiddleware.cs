using System.Net;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using HttpContextExtensions = Playwright.Training.Api.Extensions.HttpContextExtensions;

namespace Playwright.Training.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (FluentValidation.ValidationException e)
        {
            _logger.LogWarning(
                $"A validation exception was thrown with errors: [{string.Join(", ", e.Errors.Select(e => e.ErrorMessage))}]");

            await HttpContextExtensions.SetResponse(e, context, HttpStatusCode.BadRequest);
        }
        catch (JsonPatchException e)
        {
            _logger.LogWarning(
                $"A json patch exception was thrown with error: {e.Message}");
            await HttpContextExtensions.SetResponse(e, context, HttpStatusCode.BadRequest);
        }
        catch (Exception mainException)
        {
            _logger.LogError(
                $"Exception: {(mainException.InnerException != null ? mainException.Message + " -->" + mainException.InnerException.Message : mainException.Message)}");

            if (context.Response.HasStarted)
            {
                _logger.LogWarning("The response has already started, the error handler will not be executed.");
                throw;
            }

            try
            {
                await HttpContextExtensions.SetResponse(mainException, context, HttpStatusCode.InternalServerError);
            }
            catch (Exception exception)
            {
                _logger.LogError(0, exception,
                    $"The main exception handler threw while trying to report and format an error {mainException}.");
                throw;
            }
        }
    }
}