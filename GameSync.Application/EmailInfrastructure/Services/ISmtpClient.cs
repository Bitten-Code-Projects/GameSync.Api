using MailKit.Security;
using MimeKit;

namespace GameSync.Application.EmailInfrastructure;

/// <summary>
/// Represents an SMTP client for sending emails.
/// </summary>
public interface ISmtpClient : IDisposable
{
    /// <summary>
    /// Gets or sets a value indicating whether to check certificate revocation during the connection.
    /// </summary>
    bool CheckCertificateRevocation { get; set; }

    /// <summary>
    /// Connects to the SMTP server asynchronously.
    /// </summary>
    /// <param name="host">The host name of the SMTP server.</param>
    /// <param name="port">The port number to connect to.</param>
    /// <param name="options">The secure socket options to use for the connection.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous connect operation.</returns>
    Task ConnectAsync(string host, int port, SecureSocketOptions options, CancellationToken cancellationToken = default);

    /// <summary>
    /// Authenticates with the SMTP server using the provided credentials.
    /// </summary>
    /// <param name="username">The username for authentication.</param>
    /// <param name="password">The password for authentication.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous authentication operation.</returns>
    Task AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends an email message asynchronously.
    /// </summary>
    /// <param name="message">The email message to send.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous send operation.</returns>
    Task SendAsync(MimeMessage message, CancellationToken cancellationToken = default);

    /// <summary>
    /// Disconnects from the SMTP server asynchronously.
    /// </summary>
    /// <param name="quit">Indicates whether to send a QUIT command before disconnecting.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous disconnect operation.</returns>
    Task DisconnectAsync(bool quit, CancellationToken cancellationToken = default);
}