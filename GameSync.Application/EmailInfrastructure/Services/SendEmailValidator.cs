namespace GameSync.Application.EmailInfrastructure;

/// <summary>
/// Provides validation methods for the <see cref="SendEmailPayload"/> class.
/// </summary>
public static class SendEmailValidator
{
    /// <summary>
    /// Validates the SendEmailPayload to ensure that all required properties are not empty.
    /// </summary>
    /// <param name="payload">The payload containing email details.</param>
    /// <returns>
    /// Returns <c>true</c> if the sender, receiver, subject, body, and receiver email are not empty; otherwise, <c>false</c>.
    /// </returns>
 public static bool Validate(this SendEmailPayload payload) =>
    new[] { payload.Sender, payload.Receiver, payload.Subject, payload.Body, payload.ReceiverEmail }
    .All(field => !string.IsNullOrEmpty(field));
}