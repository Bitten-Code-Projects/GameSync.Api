using GameSync.Application.Shared.Commands;

namespace GameSync.Application.EmailInfrastructure;

/// <summary>
/// Provides functionality for sending emails.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends an email based on the provided payload.
    /// </summary>
    /// <param name="payload">The payload containing the email details.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="bool"/> representing the result of the email sending operation.
    /// </returns>
    Task<CommandResult> SendEmailAsync(SendEmailPayload payload, CancellationToken cancellationToken);
}
