using GameSync.Api.Middleware.Models;
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
    public async Task Invoke(HttpContext context, IOptionsSnapshot<FeatureFlags> options)
    {
        if (!options.Value.EnableRequestLogging)
        {
            await _next(context);
            return;
        }

        var originalBodyStream = context.Response.Body;

        try
        {
            // Create a new memory stream to hold the response
            using (var memoryStream = new MemoryStream())
            {
                // Set the response body to our memory stream
                context.Response.Body = memoryStream;

                // Continue down the pipeline
                await _next(context);

                // Read the response body from the memory stream
                memoryStream.Position = 0;
                var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

                if (!string.IsNullOrEmpty(responseBody))
                {
                    _logger.LogInformation($"Raw request response: {responseBody}");
                }

                // Clear the memory stream
                memoryStream.SetLength(0);

                // Reset the position to read from the beginning
                memoryStream.Position = 0;

                // Copy the modified response to the original stream
                await memoryStream.CopyToAsync(originalBodyStream);
            }
        }
        finally
        {
            // Restore the original stream
            context.Response.Body = originalBodyStream;
        }
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
