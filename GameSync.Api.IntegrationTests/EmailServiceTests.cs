namespace MyProject.Api.IntegrationTests;

using Microsoft.AspNetCore.Mvc.Testing;
using GameSync.Api;
using GameSync.Application.EmailInfrastructure;
using Moq;
using Microsoft.Extensions.Configuration;
using MailKit.Security;
using MimeKit;

public class EmailServiceTests : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task SendEmailAsync_ValidPayload_SendsEmail()
    {
        // Arrange
        var mockSmtpClient = new Mock<ISmtpClient>();
        var mockMessageFactory = new Mock<IEmailMessageFactory>();
        var mockConfiguration = new Mock<IConfiguration>();
        var emailSettings = new EmailSettings
        {
            SmtpServer = "smtp.test.com",
            SmtpPort = 587,
            AuthLogin = "test",
            Password = "test",
            SenderEmail = "sender@test.com",
            ForceTls = true
        };

        var mockSection = new Mock<IConfigurationSection>();
        mockConfiguration.Setup(x => x.GetSection("EmailSettings")).Returns(mockSection.Object);
        mockSection.Setup(x => x.GetChildren()).Returns(new List<IConfigurationSection>
        {
            new Mock<IConfigurationSection>().Object
        });

        var payload = new SendEmailPayload
        {
            Sender = "Test Sender",
            Receiver = "Test Receiver",
            ReceiverEmail = "receiver@test.com",
            Subject = "Test Subject",
            Body = "Test Body"
        };

        var emailService = new EmailService(
            mockConfiguration.Object,
            mockSmtpClient.Object,
            mockMessageFactory.Object);

        // Act
        var result = await emailService.SendEmailAsync(payload, CancellationToken.None);

        // Assert
        Assert.True(result);
        mockSmtpClient.Verify(x => x.ConnectAsync(
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<SecureSocketOptions>(),
            It.IsAny<CancellationToken>()), Times.Once);
        mockSmtpClient.Verify(x => x.SendAsync(
            It.IsAny<MimeMessage>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}