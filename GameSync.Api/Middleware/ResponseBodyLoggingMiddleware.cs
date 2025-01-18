using GameSync.Api.Middleware.Models;
using GameSync.Api.Utilities;
using Microsoft.Extensions.Options;

namespace GameSync.Api.Middleware;

/// <summary>
/// Response body logging middleware.
/// </summary>
public class ResponseBodyLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ResponseBodyLoggingMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResponseBodyLoggingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The request delegate.</param>
    /// <param name="logger">The logger.</param>
    public ResponseBodyLoggingMiddleware(RequestDelegate next, ILogger<ResponseBodyLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokation of middleware.
    /// </summary>
    /// <param name="context">The http context processed by middleware.</param>
    /// <param name="options">The options snapshot. Lets configuring during runtime.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context, IOptionsSnapshot<FeatureFlags> options)
    {
        if (!options.Value.EnableRequestLogging)
        {
            await _next(context);
            return;
        }

        var originalBodyStream = context.Response.Body;

        // Continue down the pipeline
        await _next(context);

        // Read the response body from the memory stream
        originalBodyStream.Position = 0;
        var responseBody = await new StreamReader(originalBodyStream).ReadToEndAsync();

        if (!string.IsNullOrEmpty(responseBody))
        {
            _logger.LogInformation($"Raw request response: {responseBody}".LogsSanitize());
        }

        // Reset the position to read from the beginning
        originalBodyStream.Position = 0;
    }
}

/// <summary>
/// Extension method to make it easier to add the middleware.
/// </summary>
public static class ResponseBodyMiddlewareExtensions
{
    /// <summary>
    /// Response body middleware extension method.
    /// </summary>
    /// <param name="builder">The application builder.</param>
    /// <returns>New application builder.</returns>
    public static IApplicationBuilder UseResponseBodyMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ResponseBodyLoggingMiddleware>();
    }
}
