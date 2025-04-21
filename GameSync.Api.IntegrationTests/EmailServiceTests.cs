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
        // var configuration = Substitute.For<IConfiguration>();
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
        // configuration.GetSection("EmailSettings").Returns(emailSettingsSection);
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

    // [Fact]
    // public async Task SendEmailAsync_ValidPayload_SendsEmail()
    // {
    //     // Arrange
    //     var mockSmtpClient = new Mock<ISmtpClient>();
    //     var mockMessageFactory = new Mock<IEmailMessageFactory>();
    //     var mockConfiguration = new Mock<IConfiguration>();
    //     var emailSettings = new EmailSettings
    //     {
    //         SmtpServer = "smtp.test.com",
    //         SmtpPort = 587,
    //         AuthLogin = "test",
    //         Password = "test",
    //         SenderEmail = "sender@test.com",
    //         ForceTls = true
    //     };

    //     var mockSection = new Mock<IConfigurationSection>();
    //     mockConfiguration.Setup(x => x.GetSection("EmailSettings")).Returns(mockSection.Object);
    //     mockSection.Setup(x => x.GetChildren()).Returns(new List<IConfigurationSection>
    //     {
    //         new Mock<IConfigurationSection>().Object
    //     });

    //     var payload = new SendEmailPayload
    //     {
    //         Sender = "Test Sender",
    //         Receiver = "Test Receiver",
    //         ReceiverEmail = "receiver@test.com",
    //         Subject = "Test Subject",
    //         Body = "Test Body"
    //     };

    //     var emailService = new EmailService(
    //         mockConfiguration.Object,
    //         mockSmtpClient.Object,
    //         mockMessageFactory.Object);

    //     // Act
    //     var result = await emailService.SendEmailAsync(payload, CancellationToken.None);

    //     // Assert
    //     Assert.True(result);
    //     mockSmtpClient.Verify(x => x.ConnectAsync(
    //         It.IsAny<string>(),
    //         It.IsAny<int>(),
    //         It.IsAny<SecureSocketOptions>(),
    //         It.IsAny<CancellationToken>()), Times.Once);
    //     mockSmtpClient.Verify(x => x.SendAsync(
    //         It.IsAny<MimeMessage>(),
    //         It.IsAny<CancellationToken>()), Times.Once);
    // }
}