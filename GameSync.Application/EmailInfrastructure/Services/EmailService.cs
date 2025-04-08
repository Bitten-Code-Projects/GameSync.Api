namespace GameSync.Application.EmailInfrastructure;

using EnsureThat;
using MailKit.Security;
using Microsoft.Extensions.Configuration;

/// <summary>
/// Provides functionality for sending emails using SMTP.
/// </summary>
public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ISmtpClient _smtpClient;
    private readonly IEmailMessageFactory _messageFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailService"/> class.
    /// </summary>
    /// <param name="configuration">The configuration settings.</param>
    /// <param name="smtpClient">The SMTP client used for sending emails.</param>
    /// <param name="messageFactory">The factory for creating email messages.</param>
    public EmailService(
        IConfiguration configuration,
        ISmtpClient smtpClient,
        IEmailMessageFactory messageFactory)
    {
        _configuration = configuration;
        _smtpClient = smtpClient;
        _messageFactory = messageFactory;
    }

    /// <summary>
    /// Sends an email asynchronously using the provided payload and cancellation token.
    /// </summary>
    /// <param name="payload">The payload containing email details such as sender, receiver, subject, and body.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success or failure.</returns>
    public async Task<bool> SendEmailAsync(SendEmailPayload payload, CancellationToken cancellationToken)
    {
        Ensure.That(payload).IsNotNull();

        if (!payload.Validate())
        {
            return false;
        }

        var emailSettings = _configuration.GetRequiredSection("EmailSettings").Get<EmailSettings>()
            ?? throw new InvalidOperationException("EmailSettings configuration is invalid");

        var email = _messageFactory.CreateMessage(
            payload.Sender,
            emailSettings.SenderEmail,
            payload.Receiver,
            payload.ReceiverEmail,
            payload.Subject,
            payload.Body);

        var secureSocketOptions = emailSettings.ForceTls
            ? SecureSocketOptions.SslOnConnect
            : SecureSocketOptions.StartTlsWhenAvailable;

        try
        {
            _smtpClient.CheckCertificateRevocation = false;
            await _smtpClient.ConnectAsync(emailSettings.SmtpServer, emailSettings.SmtpPort, secureSocketOptions, cancellationToken);
            await _smtpClient.AuthenticateAsync(emailSettings.AuthLogin, emailSettings.Password, cancellationToken);
            await _smtpClient.SendAsync(email, cancellationToken);
            await _smtpClient.DisconnectAsync(true, cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }
}