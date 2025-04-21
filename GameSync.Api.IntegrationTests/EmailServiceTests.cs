namespace MyProject.Api.IntegrationTests;

using Microsoft.AspNetCore.Mvc.Testing;
using GameSync.Api;
using GameSync.Application.EmailInfrastructure;
using Microsoft.Extensions.Configuration;
using MailKit.Security;
using MimeKit;
using NSubstitute;

public class EmailServiceTests : IClassFixture<WebApplicationFactory<Program>>
{

    [Fact]
    public async Task SendEmailAsync_ValidPayload_SendsEmail()
    {
        // Arrange
        var smtpClient = Substitute.For<ISmtpClient>();
        var emailMessageFactory = Substitute.For<IEmailMessageFactory>();
        var inMemorySettings = new Dictionary<string, string>
        {
            {"EmailSettings:SmtpServer", "mail.ugu.pl"},
            {"EmailSettings:SmtpPort", "587"},
            {"EmailSettings:SenderEmail", "example@domain.com"},
            {"EmailSettings:AuthLogin", "example-login"},
            {"EmailSettings:Password", "example-password"},
            {"EmailSettings:ForceTls", "true"}
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();
        

        // Konfiguracja sekcji "EmailSettings"
        var emailSettingsSection = Substitute.For<IConfigurationSection>();
        emailSettingsSection.GetChildren().Returns(new List<IConfigurationSection>
    {
        Substitute.For<IConfigurationSection>()
    });

        var payload = new SendEmailPayload
        {
            Sender = "Test Sender",
            Receiver = "Test Receiver",
            ReceiverEmail = "receiver@test.com",
            Subject = "Test Subject",
            Body = "Test Body"
        };

        var emailService = new EmailService(configuration, smtpClient, emailMessageFactory);

        // Act
        var result = await emailService.SendEmailAsync(payload, CancellationToken.None);

        // Assert
        Assert.True(result);
        await smtpClient.Received(1).ConnectAsync(
            Arg.Any<string>(),
            Arg.Any<int>(),
            Arg.Any<SecureSocketOptions>(),
            Arg.Any<CancellationToken>());
        await smtpClient.Received(1).SendAsync(
            Arg.Any<MimeMessage>(),
            Arg.Any<CancellationToken>());
    }
}