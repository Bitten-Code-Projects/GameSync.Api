using GameSync.Api.IntegrationTests.Utilities;
using GameSync.Application.Account.UseCases.ConfirmEmail;
using GameSync.Domain.Shared.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.WebUtilities;
using NSubstitute;
using NSubstitute.Extensions;

namespace GameSync.Api.IntegrationTests.EmailConfirmation;

public class EmailConfirmationTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly IRequestHandler<ConfirmEmailCommand, CommandResult> _mockConfirmEmailHandler = Substitute.For<IRequestHandler<ConfirmEmailCommand, CommandResult>>();
    
    [Fact]
    public async Task EmailConfirmation_WhenConfirmEmailCommandReturnsSuccess_ShouldReturnHttpStatusCodeOk()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var code = "code-for-email-confirmation";
        
        _mockConfirmEmailHandler
            .Handle(
                Arg.Is(new ConfirmEmailCommand(userId, code)),
                Arg.Any<CancellationToken>()
            )
            .Returns(CommandResult.Success);
        
        var client = ClientFactory.GetHttpClientWithMocks(factory, new Dictionary<Type, object>
        {
            { typeof(IRequestHandler<ConfirmEmailCommand, CommandResult>), _mockConfirmEmailHandler }
        });
        
        // Act
        var encodedCode = WebEncoders.Base64UrlEncode(System.Text.Encoding.UTF8.GetBytes(code));
        var response = await client.GetAsync($"/api/account/confirm-email?userId={userId}&code={encodedCode}");
         
        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task EmailConfirmation_WhenConfirmEmailCommandReturnsError_ShouldReturnHttpStatusCodeBadRequestWithMessage()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var code = "code-for-email-confirmation";
        
        _mockConfirmEmailHandler
            .Handle(
                Arg.Is(new ConfirmEmailCommand(userId, code)),
                Arg.Any<CancellationToken>()
            )
            .Returns(CommandResult.Fail("User not found."));
        
        var client = ClientFactory.GetHttpClientWithMocks(factory, new Dictionary<Type, object>
        {
            { typeof(IRequestHandler<ConfirmEmailCommand, CommandResult>), _mockConfirmEmailHandler }
        });
        
        // Act
        var encodedCode = WebEncoders.Base64UrlEncode(System.Text.Encoding.UTF8.GetBytes(code));
        var response = await client.GetAsync($"/api/account/confirm-email?userId={userId}&code={encodedCode}");
        var deserializedResponse = await response.Content.ReadAsStringAsync();
         
        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(deserializedResponse);
        Assert.Equal("User not found.", deserializedResponse);
    }
}