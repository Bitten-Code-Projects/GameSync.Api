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
    /// Configuration of exception middleware.
    /// </summary>
    /// <param name="app">Application builder.</param>
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
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
                            break;
                        default:
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            message = $"Internal Server Error";
                            // todo: save error to db
                            break;
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