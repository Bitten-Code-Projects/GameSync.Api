using MimeKit;

namespace GameSync.Application.EmailInfrastructure;

/// <summary>
/// Defines a factory for creating email messages.
/// </summary>
public interface IEmailMessageFactory
{
    /// <summary>
    /// Creates an email message with the specified details.
    /// </summary>
    /// <param name="from">The name of the sender.</param>
    /// <param name="fromEmail">The email address of the sender.</param>
    /// <param name="to">The name of the recipient.</param>
    /// <param name="toEmail">The email address of the recipient.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="body">The body content of the email.</param>
    /// <returns>A <see cref="MimeMessage"/> representing the email message.</returns>
    MimeMessage CreateMessage(string from, string fromEmail, string to, string toEmail, string subject, string body);
}