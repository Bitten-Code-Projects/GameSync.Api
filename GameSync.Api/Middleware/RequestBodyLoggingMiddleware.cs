using System.Text;
using GameSync.Api.Middleware.Models;
using Microsoft.Extensions.Options;

namespace GameSync.Api.Middleware;

public class RequestBodyLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public RequestBodyLoggingMiddleware(RequestDelegate next, ILogger<RequestBodyLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IOptionsSnapshot<FeatureFlags> options)
    {
        if (!options.Value.EnableRequestLogging)
        {
            await _next(context);
            return;
        }

        // Enable buffering so the body can be read multiple times
        context.Request.EnableBuffering();

        _logger.LogInformation($"Raw request path: {context.Request.Method} {context.Request.Path}{context.Request.QueryString}");

        // Read the request body
        using var requestStreamReader = new StreamReader(
            context.Request.Body,
            encoding: Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            leaveOpen: true);

        var rawRequestBody = await requestStreamReader.ReadToEndAsync();

        // Log or process the raw body as needed
        if (!string.IsNullOrEmpty(rawRequestBody))
        {
            _logger.LogInformation($"Raw request body: {rawRequestBody}");
        }

        // IMPORTANT: Reset the request body position for other middleware and controllers
        context.Request.Body.Position = 0;

        // Call the next middleware in the pipeline
        await _next(context);
    }
}

// Extension method to make registration cleaner
public static class RequestBodyLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestBodyLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestBodyLoggingMiddleware>();
    }
}
