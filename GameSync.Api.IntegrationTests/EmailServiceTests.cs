using Microsoft.AspNetCore.Mvc.Testing;
using GameSync.Api;
using GameSync.Application.EmailInfrastructure;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace MyProject.Api.IntegrationTests;

public class EmailServiceTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly IEmailService _emailService;

    private readonly SendEmailPayload _command = new SendEmailPayload
    {
        Sender = "bcp@bittencodeprojects.ugu.pl",
        Receiver = "bcp@bittencodeprojects.ugu.pl",
        Subject = "Test Email",
        Body = "This is a test email.",
        ReceiverEmail = "bcp@bittencodeprojects.ugu.pl"
    };

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
    public async Task SendEmail_ShouldCallEmailService_WhenEmailIsSentSuccessfully()
    {
        _emailService.SendEmailAsync(Arg.Any<SendEmailPayload>(), Arg.Any<CancellationToken>())
                     .Returns(Task.FromResult(true));

        // Act
        var result = await _emailService.SendEmailAsync(_command, CancellationToken.None);

        // Assert
        Assert.True(result);
        await _emailService.Received(1).SendEmailAsync(Arg.Any<SendEmailPayload>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SendEmail_ShouldThrowException_WhenEmailSendingFails()
    {
        _emailService.SendEmailAsync(Arg.Any<SendEmailPayload>(), Arg.Any<CancellationToken>())
               .Returns(Task.FromException<bool>(new Exception("Email sending failed")));

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(async () =>
        {
            await _emailService.SendEmailAsync(_command, CancellationToken.None);
        });

        // Assert
        Assert.Equal("Email sending failed", exception.Message);
        await _emailService.Received(1).SendEmailAsync(Arg.Any<SendEmailPayload>(), Arg.Any<CancellationToken>());
    }

}