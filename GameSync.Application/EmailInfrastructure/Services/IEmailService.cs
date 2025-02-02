using GameSync.Domain.Shared.Commands;

namespace GameSync.Application.EmailInfrastructure;

/// <summary>
/// Provides functionality for sending emails.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends an email based on the provided command.
    /// </summary>
    /// <param name="command">The command containing the email details.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="CommandResult"/> representing the result of the email sending operation.
    /// </returns>
    Task<CommandResult> SendEmailAsync(SendEmailCommand command, CancellationToken cancellationToken);
}
