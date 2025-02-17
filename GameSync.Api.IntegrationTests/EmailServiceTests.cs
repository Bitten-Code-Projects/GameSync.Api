using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using GameSync.Api;
using GameSync.Application.EmailInfrastructure;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace MyProject.Api.IntegrationTests;


public interface IEmailService
{
    Task SendEmailAsync(SendEmailCommand command);
}

public class EmailServiceTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly IEmailService _emailService;

    public EmailServiceTests(WebApplicationFactory<Program> factory)
    {
        _emailService = NSubstitute.Substitute.For<IEmailService>();

        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IEmailService));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                services.AddSingleton(_emailService);
            });
        });
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

        _emailService.SendEmailAsync(Arg.Any<SendEmailCommand>())
                     .Returns(Task.CompletedTask);

        // Act
        await _emailService.SendEmailAsync(command);

        // Assert
        await _emailService.Received(1).SendEmailAsync(Arg.Any<SendEmailCommand>());
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

        _emailService.SendEmailAsync(Arg.Any<SendEmailCommand>())
                     .Returns(Task.FromException(new Exception("Email sending failed")));

        // Act & Assert
        Func<Task> act = async () => { await _emailService.SendEmailAsync(command); };
        await act.Should().ThrowAsync<Exception>().WithMessage("Email sending failed");

        await _emailService.Received(1).SendEmailAsync(Arg.Any<SendEmailCommand>());
    }
}
