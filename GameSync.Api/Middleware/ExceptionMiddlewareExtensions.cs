using System.Net;
using FluentValidation;
using GameSync.Api.Shared.Middleware.Models;
using GameSync.Domain.Shared.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace GameSync.Api.Shared.Middleware;

/// <summary>
/// Middleware for handling exceptions.
/// </summary>
public static class ExceptionMiddlewareExtensions
{
    /// <summary>
    /// Warning status codes.
    /// </summary>
    private readonly static int[] WarningStatusCodes = [400];

    /// <summary>
    /// Error status codes.
    /// </summary>
    private readonly static int[] ErrorStatusCodes = [500];

    /// <summary>
    /// Configuration of exception middleware.
    /// </summary>
    /// <param name="app">Application builder.</param>
    /// <param name="logger">Logger provider.</param>
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger<Program> logger)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    string message = string.Empty;
                    List<string> errorsList = new List<string>();
                    switch (contextFeature.Error)
                    {
                        case NotFoundException ex:
                            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            message = ex.Message;
                            break;
                        case ValidationException ex:
                            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            message = "Validation errors";
                            errorsList = ex.Errors.Select(x => x.ErrorMessage).ToList();
                            // todo: prevent validation exception logging as an error
                            break;
                        default:
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            message = $"Internal Server Error";
                            break;
                    }

                    if (WarningStatusCodes.Any(x => x == context.Response.StatusCode))
                    {
                        logger.LogWarning("Returned {HttpCode} with message: {Message}", context.Response.StatusCode, message);
                        if (errorsList.Any())
                        {
                            logger.LogWarning("Validation errors: {ValidationErrors}", string.Join(", ", errorsList));
                        }
                    }
                    else if (ErrorStatusCodes.Any(x => x == context.Response.StatusCode))
                    {
                        logger.LogError("Returned {HttpCode} with message: {Message} {ExceptionMessage} {StackTrace}", context.Response.StatusCode, message, contextFeature.Error.Message, contextFeature.Error.StackTrace);
                    }
                    else
                    {
                        logger.LogInformation("Returned {HttpCode} with message: {Message}", context.Response.StatusCode, message);
                    }

                    await context.Response.WriteAsync(new ErrorDetails()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = message,
                        ValidationErrors = errorsList,
                    }.ToString());
                }
            });
        });
    }
}