namespace GameSync.Application.EmailInfrastructure;

/// <summary>
/// Represents the configuration settings for an email service.
/// </summary>
public class EmailSettings
{
    /// <summary>
    /// Gets or sets the SMTP server address (e.g., "smtp.example.com").
    /// </summary>
    public required string SmtpServer { get; set; }

    /// <summary>
    /// Gets or sets the port number used by the SMTP server.
    /// Common values are 25, 465, or 587.
    /// </summary>
    public required int SmtpPort { get; set; }

    /// <summary>
    /// Gets or sets the email address used as the sender in outgoing emails.
    /// </summary>
    public required string SenderEmail { get; set; }

    /// <summary>
    /// Gets or sets the login to authenticate when sending email.
    /// </summary>
    public required string AuthLogin { get; set; }

    /// <summary>
    /// Gets or sets the password to authenticate when sending email.
    /// </summary>
    public required string Password { get; set; }
}