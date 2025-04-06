namespace GameSync.Application.EmailInfrastructure;

using EnsureThat;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

/// <summary>
/// Provides functionality for sending emails.
/// </summary>
public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailService"/> class using the specified configuration.
    /// </summary>
    /// <param name="configuration">The application configuration settings.</param>
    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Sends an email based on the provided payload.
    /// </summary>
    /// <param name="payload">The payload containing the email details such as sender, receiver, subject, and body.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="bool"/> indicating the success or failure of the email sending operation.
    /// </returns>
    /// <remarks>
    /// This method first validates the payload using custom validation logic. If the payload is invalid,
    /// it returns a failure result with an error message. Otherwise, it retrieves the email settings from the configuration,
    /// constructs a <see cref="MimeMessage"/>, and sends the email using an SMTP client. If the email is sent successfully,
    /// a success result is returned; if any error occurs during the process, a failure result is returned.
    /// </remarks>
    public async Task<bool> SendEmailAsync(SendEmailPayload payload, CancellationToken cancellationToken)
{
    Ensure.That(payload).IsNotNull();

    bool validationResult = payload.Validate();

    if (!validationResult)
    {
        return false;
    }

    var emailSettings = _configuration.GetRequiredSection("EmailSettings").Get<EmailSettings>()
    ?? throw new InvalidOperationException("EmailSettings configuration is invalid");

    var email = new MimeMessage();
    email.From.Add(new MailboxAddress(payload.Sender, emailSettings.SenderEmail));
    email.To.Add(new MailboxAddress(payload.Receiver, payload.ReceiverEmail));
    email.Subject = payload.Subject;
    email.Body = new TextPart("plain")
    {
        Text = payload.Body,
    };

    var secureSocketOptions = emailSettings.ForceTls
        ? MailKit.Security.SecureSocketOptions.SslOnConnect
        : MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable;

    using (var smtpClient = new SmtpClient())
    {
        smtpClient.CheckCertificateRevocation = false;
        await smtpClient.ConnectAsync(emailSettings.SmtpServer, emailSettings.SmtpPort, secureSocketOptions, cancellationToken);
        await smtpClient.AuthenticateAsync(emailSettings.AuthLogin, emailSettings.Password, cancellationToken);
        await smtpClient.SendAsync(email, cancellationToken);
        await smtpClient.DisconnectAsync(true, cancellationToken);
    }

    return true;
}
    // public async Task<bool> SendEmailAsync(SendEmailPayload payload, CancellationToken cancellationToken)
    // {
    //     Ensure.That(payload).IsNotNull();

    //     bool validationResult = payload.Validate();

    //     if (!validationResult)
    //     {
    //         return false;
    //     }

    //     var emailSettings = _configuration.GetRequiredSection("EmailSettings").Get<EmailSettings>()
    //     ?? throw new InvalidOperationException("EmailSettings configuration is invalid");

    //     var email = new MimeMessage();
    //     email.From.Add(new MailboxAddress(payload.Sender, emailSettings.SenderEmail));
    //     email.To.Add(new MailboxAddress(payload.Receiver, payload.ReceiverEmail));
    //     email.Subject = payload.Subject;
    //     email.Body = new TextPart("plain")
    //     {
    //         Text = payload.Body,
    //     };

    //     using (var smtpClient = new SmtpClient())
    //     {
    //         smtpClient.CheckCertificateRevocation = false;
    //         await smtpClient.ConnectAsync(emailSettings.SmtpServer, emailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable, cancellationToken);
    //         await smtpClient.AuthenticateAsync(emailSettings.AuthLogin, emailSettings.Password, cancellationToken);
    //         await smtpClient.SendAsync(email, cancellationToken);
    //         await smtpClient.DisconnectAsync(true, cancellationToken);
    //     }

    //     return true;
    // }
}
