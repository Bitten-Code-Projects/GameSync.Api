using FluentValidation;
using GameSync.Api.Domain.Shared.Exceptions;
using GameSync.Api.Domain.Shared.Middleware.Models;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace GameSync.Api.Domain.Shared.Middleware;

public static class ExceptionMiddlewareExtensions
{
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
                    string messagePrefix = $"Path: {context.Request.Path} IP: {context.Request.HttpContext.Connection.RemoteIpAddress} Message: "; // todo: body?
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