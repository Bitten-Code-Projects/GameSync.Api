namespace GameSync.Application.EmailInfrastructure;

/// <summary>
/// Represents the payload containing the necessary details to send an email.
/// </summary>
public class SendEmailPayload
{
    /// <summary>
    /// Gets or sets the email alias of the sender.
    /// </summary>
    public required string Sender { get; set; }

    /// <summary>
    /// Gets or sets the email alias of the receiver.
    /// </summary>
    public required string Receiver { get; set; }

    /// <summary>
    /// Gets or sets the email address of the receiver.
    /// </summary>
    public required string ReceiverEmail { get; set; }

    /// <summary>
    /// Gets or sets the subject of the email.
    /// </summary>
    public required string Subject { get; set; }

    /// <summary>
    /// Gets or sets the body content of the email.
    /// </summary>
    public required string Body { get; set; }
}
