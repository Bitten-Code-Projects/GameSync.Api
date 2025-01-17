using GameSync.Api.Middleware.Models;
using GameSync.Api.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSync.Api.IntegrationTests.Global.Middleware;

public class ResponseBodyLoggingMiddlewareTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly IOptionsSnapshot<FeatureFlags> _snapshot = Substitute.For<IOptionsSnapshot<FeatureFlags>>();
    private readonly ILogger<ResponseBodyLoggingMiddleware> _logger = Substitute.For<ILogger<ResponseBodyLoggingMiddleware>>();

    [Fact]
    public async Task MiddlewareInvokationLogger_DoesntRun_WhenDefaultHttpContext()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var nextDelegate = new RequestDelegate(_ => Task.CompletedTask);
        var middleware = new ResponseBodyLoggingMiddleware(nextDelegate, _logger);
        _snapshot.Value.Returns(new FeatureFlags { EnableRequestLogging = true });

        // Act
        await middleware.InvokeAsync(context, _snapshot);

        // Assert
        _logger.ReceivedWithAnyArgs(0).Log(LogLevel.Information, default, default, default, default!);
    }

    [Fact]
    public async Task MiddlewareInvokationLogger_RunsTwice_WhenBodyIsNotEmpty()
    {
        // Arrange
        const string TestBody = "test body";
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(TestBody));

        var nextDelegate = new RequestDelegate(_ => Task.CompletedTask);
        var middleware = new ResponseBodyLoggingMiddleware(nextDelegate, _logger);
        _snapshot.Value.Returns(new FeatureFlags { EnableRequestLogging = true });

        // Act
        await middleware.InvokeAsync(context, _snapshot);

        // Assert
        _logger.ReceivedWithAnyArgs(1).Log(LogLevel.Information, default, default, default, default!);
    }

    [Fact]
    public async Task MiddlewareInvokationLogger_ReturnsBody_WhenBodyIsNotEmpty()
    {
        // Arrange
        const string TestBody = "test body";
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(TestBody));

        var nextDelegate = new RequestDelegate(_ => Task.CompletedTask);
        var middleware = new ResponseBodyLoggingMiddleware(nextDelegate, _logger);
        _snapshot.Value.Returns(new FeatureFlags { EnableRequestLogging = true });

        // Act
        await middleware.InvokeAsync(context, _snapshot);

        // Assert
        _logger.Received().Log(LogLevel.Information, $"Raw request response: {TestBody}");
    }

    [Fact]
    public async Task MiddlewareInvokationLogger_DoesntWork_WhenDisabledInAppsettings()
    {
        // Arrange
        var context = new DefaultHttpContext();

        var nextDelegate = new RequestDelegate(_ => Task.CompletedTask);
        var middleware = new ResponseBodyLoggingMiddleware(nextDelegate, _logger);
        _snapshot.Value.Returns(new FeatureFlags { EnableRequestLogging = false });

        // Act
        await middleware.InvokeAsync(context, _snapshot);

        // Assert
        _logger.ReceivedWithAnyArgs(0).Log(LogLevel.Information, default, default, default, default!);
    }
}
