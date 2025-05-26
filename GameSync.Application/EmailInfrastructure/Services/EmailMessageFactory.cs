using MimeKit;

namespace GameSync.Application.EmailInfrastructure;

/// <summary>
/// Factory class for creating email messages.
/// </summary>
public class EmailMessageFactory : IEmailMessageFactory
{
    /// <summary>
    /// Creates an email message with the specified sender, recipient, subject, and body.
    /// </summary>
    /// <param name="from">The name of the sender.</param>
    /// <param name="fromEmail">The email address of the sender.</param>
    /// <param name="to">The name of the recipient.</param>
    /// <param name="toEmail">The email address of the recipient.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="body">The body content of the email.</param>
    /// <returns>A <see cref="MimeMessage"/> representing the email message.</returns>
    public MimeMessage CreateMessage(string from, string fromEmail, string to, string toEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(from, fromEmail));
        message.To.Add(new MailboxAddress(to, toEmail));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };
        return message;
    }
}