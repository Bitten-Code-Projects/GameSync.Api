namespace GameSync.Application.EmailInfrastructure.UseCases;

using GameSync.Domain.Shared.Commands;
using MediatR;

/// <summary>
/// Represents a command to send an email.
/// This class implements IRequest with CommandResult for use with MediatR.
/// </summary>
public class SendEmailCommand : IRequest<CommandResult>
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
