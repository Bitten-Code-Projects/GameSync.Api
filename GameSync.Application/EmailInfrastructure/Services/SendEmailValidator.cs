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
    public static bool Validate(this SendEmailPayload payload)
    {
        if (payload.Sender.IsNullOrEmpty() || payload.Receiver.IsNullOrEmpty() || payload.Subject.IsNullOrEmpty() || payload.Body.IsNullOrEmpty() || payload.ReceiverEmail.IsNullOrEmpty())
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Determines whether the specified string is null or an empty string.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns>
    /// Returns <c>true</c> if the string is null or empty; otherwise, <c>false</c>.
    /// </returns>
    private static bool IsNullOrEmpty(this string value) {
        return string.IsNullOrEmpty(value);
    }
}