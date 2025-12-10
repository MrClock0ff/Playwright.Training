using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Playwright.Training.Api.Extensions;

public static class HttpContextExtensions
    {
        static readonly Func<HttpStatusCode, Exception, string> ExceptionResponseMessage = (statusCode, exception) => statusCode == HttpStatusCode.InternalServerError ? $"Internal server error. [{(exception.InnerException != null ? exception.Message + " -->" + exception.InnerException.Message : exception.Message)}]" : exception.Message;

        public static async Task SetResponse(Exception exception, HttpContext context, HttpStatusCode statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(exception.ResponseMessage(statusCode));
        }

        private static string ResponseMessage(this Exception exception, HttpStatusCode statusCode)
        {
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            ProblemDetails problemDetails = (exception is FluentValidation.ValidationException validationException) ? new ValidationProblemDetails() : new ProblemDetails();

            problemDetails.Type = ResolveErrorCodeTypeLink(statusCode);
            problemDetails.Title = exception.Message;
            problemDetails.Detail = ExceptionResponseMessage(statusCode, exception);
            problemDetails.Status = (int)statusCode;

            if (exception.GetType() == typeof(FluentValidation.ValidationException))
            {
                foreach (var error in ((FluentValidation.ValidationException)exception).Errors)
                {
                    ((ValidationProblemDetails)problemDetails).Errors.Add(error.PropertyName, new string[1] { error.ErrorMessage });
                }
            }

            return JsonSerializer.Serialize(problemDetails, serializeOptions);
        }

        private static string ResolveErrorCodeTypeLink(HttpStatusCode statusCode) =>
            statusCode switch
            {
                HttpStatusCode.BadRequest => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                HttpStatusCode.Unauthorized => "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
                HttpStatusCode.Forbidden => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3",
                HttpStatusCode.NotFound => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                HttpStatusCode.Gone => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.9",
                HttpStatusCode.InternalServerError => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                _ => string.Empty,
            };
    }