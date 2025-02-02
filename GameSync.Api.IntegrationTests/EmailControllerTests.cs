using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using GameSync.Api;
using GameSync.Application.EmailInfrastructure.UseCases;
using System.Net.Http.Json;
using GameSync.Application.EmailInfrastructure;

namespace MyProject.Api.IntegrationTests;

public class EmailControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public EmailControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SendEmail_ReturnsOk_WhenEmailSentSuccessfully()
    {
        // Arrange
        var command = new SendEmailCommand
        {
            Sender = "bcp@bittencodeprojects.ugu.pl",
            Receiver = "bcp@bittencodeprojects.ugu.pl",
            Subject = "Test Email",
            Body = "This is a test email.",
            ReceiverEmail = "bcp@bittencodeprojects.ugu.pl"
        };

        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/email/send", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task SendEmail_ReturnsBadRequest_WhenEmailSendingFails()
    {
        // Arrange
        var command = new SendEmailCommand
        {
            Sender = "bcp2@bittencodeprojects.ugu.pl",
            Receiver = "bcp2@bittencodeprojects.ugu.pl",
            Subject = "Test Email",
            Body = "This is a test email.",
            ReceiverEmail = "bcp2@bittencodeprojects.ugu.pl"
        };

        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/email/send", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
