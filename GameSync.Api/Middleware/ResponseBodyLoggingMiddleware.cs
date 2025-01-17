using GameSync.Api.Middleware.Models;
using Microsoft.Extensions.Options;

namespace GameSync.Api.Middleware;

public class ResponseBodyLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ResponseBodyLoggingMiddleware> _logger;

    public ResponseBodyLoggingMiddleware(RequestDelegate next, ILogger<ResponseBodyLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

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

// Extension method to make it easier to add the middleware
public static class ResponseBodyMiddlewareExtensions
{
    public static IApplicationBuilder UseResponseBodyMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ResponseBodyLoggingMiddleware>();
    }
}
