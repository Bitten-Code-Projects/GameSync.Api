using System.Text;
using GameSync.Api.Middleware.Models;
using Microsoft.Extensions.Options;

namespace GameSync.Api.Middleware;

/// <summary>
/// Request body logging middleware.
/// </summary>
public class RequestBodyLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestBodyLoggingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The request delegate.</param>
    /// <param name="logger">The logger.</param>
    public RequestBodyLoggingMiddleware(RequestDelegate next, ILogger<RequestBodyLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokation of middleware.
    /// </summary>
    /// <param name="context">The http context processed by middleware.</param>
    /// <param name="options">The options snapshot. Lets configuring during runtime.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

/// <summary>
/// Extension method to make it easier to add the middleware.
/// </summary>
public static class RequestBodyLoggingMiddlewareExtensions
{
    /// <summary>
    /// Response body middleware extension method.
    /// </summary>
    /// <param name="builder">The application builder.</param>
    /// <returns>New application builder.</returns>
    public static IApplicationBuilder UseRequestBodyLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestBodyLoggingMiddleware>();
    }
}
