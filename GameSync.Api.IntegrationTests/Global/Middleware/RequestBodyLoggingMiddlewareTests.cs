using GameSync.Api.Middleware;
using GameSync.Api.Middleware.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace GameSync.Api.IntegrationTests.Global.Middleware;

public class RequestBodyLoggingMiddlewareTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly IOptionsSnapshot<FeatureFlags> _snapshot = Substitute.For<IOptionsSnapshot<FeatureFlags>>();
    private readonly ILogger<RequestBodyLoggingMiddleware> _logger = Substitute.For<ILogger<RequestBodyLoggingMiddleware>>();

    [Fact]
    public async Task MiddlewareInvokationLogger_RunsOnce_WhenDefaultHttpContext()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var nextDelegate = new RequestDelegate(_ => Task.CompletedTask);
        var middleware = new RequestBodyLoggingMiddleware(nextDelegate, _logger);
        _snapshot.Value.Returns(new FeatureFlags { EnableRequestLogging = true });

        // Act
        await middleware.InvokeAsync(context, _snapshot);

        // Assert
        _logger.ReceivedWithAnyArgs(1).Log(LogLevel.Information, default, default, default, default!);
    }

    [Fact]
    public async Task MiddlewareInvokationLogger_RunsTwice_WhenBodyIsNotEmpty()
    {
        // Arrange
        const string TestBody = "test body";
        var context = new DefaultHttpContext();
        context.Request.Body = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(TestBody));

        var nextDelegate = new RequestDelegate(_ => Task.CompletedTask);
        var middleware = new RequestBodyLoggingMiddleware(nextDelegate, _logger);
        _snapshot.Value.Returns(new FeatureFlags { EnableRequestLogging = true });

        // Act
        await middleware.InvokeAsync(context, _snapshot);

        // Assert
        _logger.ReceivedWithAnyArgs(2).Log(LogLevel.Information, default, default, default, default!);
    }

    [Fact]
    public async Task MiddlewareInvokationLogger_ReturnsRequest_WhenRequestIsNotEmpty()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Method = "GET";
        context.Request.Path = "/test";
        context.Request.QueryString = new QueryString("?test=1");

        var nextDelegate = new RequestDelegate(_ => Task.CompletedTask);
        var middleware = new RequestBodyLoggingMiddleware(nextDelegate, _logger);
        _snapshot.Value.Returns(new FeatureFlags { EnableRequestLogging = true });

        // Act
        await middleware.InvokeAsync(context, _snapshot);

        // Assert
        _logger.Received().Log(LogLevel.Information, $"Raw request path: {context.Request.Method} {context.Request.Path}{context.Request.QueryString}");
    }

    [Fact]
    public async Task MiddlewareInvokationLogger_ReturnsBody_WhenBodyIsNotEmpty()
    {
        // Arrange
        const string TestBody = "test body";
        var context = new DefaultHttpContext();
        context.Request.Body = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(TestBody));

        var nextDelegate = new RequestDelegate(_ => Task.CompletedTask);
        var middleware = new RequestBodyLoggingMiddleware(nextDelegate, _logger);
        _snapshot.Value.Returns(new FeatureFlags { EnableRequestLogging = true });

        // Act
        await middleware.InvokeAsync(context, _snapshot);

        // Assert
        _logger.Received().Log(LogLevel.Information, $"Raw request body: {TestBody}");
    }

    [Fact]
    public async Task MiddlewareInvokationLogger_DoesntWork_WhenDisabledInAppsettings()
    {
        // Arrange
        var context = new DefaultHttpContext();

        var nextDelegate = new RequestDelegate(_ => Task.CompletedTask);
        var middleware = new RequestBodyLoggingMiddleware(nextDelegate, _logger);
        _snapshot.Value.Returns(new FeatureFlags { EnableRequestLogging = false });

        // Act
        await middleware.InvokeAsync(context, _snapshot);

        // Assert
        _logger.ReceivedWithAnyArgs(0).Log(LogLevel.Information, default, default, default, default!);
    }
}
