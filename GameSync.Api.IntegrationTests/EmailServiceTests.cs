namespace MyProject.Api.IntegrationTests;

using Microsoft.AspNetCore.Mvc.Testing;
using GameSync.Api;
using GameSync.Application.EmailInfrastructure;
using Microsoft.Extensions.Configuration;
using MailKit.Security;
using MimeKit;
using NSubstitute;
using Shouldly;
using Microsoft.Extensions.Logging;
using NSubstitute.ExceptionExtensions;
using GameSync.Domain.Shared.Commands;

public class EmailServiceTests : IClassFixture<WebApplicationFactory<Program>>
{

    [Fact]
    public async Task SendEmailAsync_ValidPayload_SendsEmail()
    {
        // Arrange
        var smtpClient = Substitute.For<ISmtpClient>();
        var logger = Substitute.For<ILogger<EmailService>>();
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

        var emailService = new EmailService(configuration, smtpClient, logger, emailMessageFactory);

        // Act
        var result = await emailService.SendEmailAsync(payload, CancellationToken.None);

        // Assert
        result.ShouldBe(CommandResult.Success);

        await smtpClient.Received(1).ConnectAsync(
            Arg.Any<string>(),
            Arg.Any<int>(),
            Arg.Any<SecureSocketOptions>(),
            Arg.Any<CancellationToken>());

        await smtpClient.Received(1).SendAsync(
            Arg.Any<MimeMessage>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SendEmailAsync_InvalidPayload_DoesNotSendEmail()
    {
        // Arrange
        var smtpClient = Substitute.For<ISmtpClient>();
        var logger = Substitute.For<ILogger<EmailService>>();
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

        var payload = new SendEmailPayload
        {
            Sender = "Test Sender",
            Receiver = "Test Receiver",
            ReceiverEmail = "receiver@test.com",
            Subject = "Test Subject",
            Body = "Test Body"
        };

        var emailService = new EmailService(configuration, smtpClient, logger, emailMessageFactory);

        smtpClient.SendAsync(Arg.Any<MimeMessage>(), Arg.Any<CancellationToken>())
            .Throws(new Exception("Sending email failed"));

        // Act
        var result = await emailService.SendEmailAsync(payload, CancellationToken.None);

        // Assert
        result.ShouldBeEquivalentTo(CommandResult.Fail("Sending email failed"));

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