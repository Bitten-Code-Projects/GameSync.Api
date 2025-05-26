using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace GameSync.Application.EmailInfrastructure;

/// <summary>
/// Wrapper class for the SmtpClient, implementing the ISmtpClient interface.
/// </summary>
public class SmtpClientWrapper : ISmtpClient
{
    private readonly SmtpClient _smtpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="SmtpClientWrapper"/> class.
    /// </summary>
    public SmtpClientWrapper()
    {
        _smtpClient = new SmtpClient();
    }

    /// <summary>
    /// Gets or sets a value indicating whether to check the certificate revocation.
    /// </summary>
    public bool CheckCertificateRevocation
    {
        get => _smtpClient.CheckCertificateRevocation;
        set => _smtpClient.CheckCertificateRevocation = value;
    }

    /// <summary>
    /// Connects to the SMTP server using the specified host, port, and secure socket options.
    /// </summary>
    /// <param name="host">The host name or IP address of the SMTP server.</param>
    /// <param name="port">The port number to connect to on the SMTP server.</param>
    /// <param name="options">The secure socket options to use when connecting to the SMTP server.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous connect operation.</returns>
    public async Task ConnectAsync(string host, int port, SecureSocketOptions options, CancellationToken cancellationToken = default)
        => await _smtpClient.ConnectAsync(host, port, options, cancellationToken);

    /// <summary>
    /// Authenticates the client with the specified username and password.
    /// </summary>
    /// <param name="username">The username to authenticate with the SMTP server.</param>
    /// <param name="password">The password to authenticate with the SMTP server.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous authentication operation.</returns>
    public async Task AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default)
        => await _smtpClient.AuthenticateAsync(username, password, cancellationToken);

    /// <summary>
    /// Sends the specified MIME message asynchronously.
    /// </summary>
    /// <param name="message">The MIME message to be sent.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous send operation.</returns>
    public async Task SendAsync(MimeMessage message, CancellationToken cancellationToken = default)
        => await _smtpClient.SendAsync(message, cancellationToken);

    /// <summary>
    /// Disconnects from the SMTP server. If quit is true, it will send a QUIT command to the server.
    /// </summary>
    /// <param name="quit">Indicates whether to send a QUIT command to the server before disconnecting.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous disconnect operation.</returns>
    public async Task DisconnectAsync(bool quit, CancellationToken cancellationToken = default)
        => await _smtpClient.DisconnectAsync(quit, cancellationToken);

    /// <summary>
    /// Disposes the SmtpClient and releases any resources it holds.
    /// </summary>
    public void Dispose()
    {
        _smtpClient?.Dispose();
    }
}