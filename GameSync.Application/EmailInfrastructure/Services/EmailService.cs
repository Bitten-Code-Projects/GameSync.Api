using EnsureThat;
using FluentValidation;
using FluentValidation.Results;
using GameSync.Domain.Shared.Commands;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace GameSync.Application.EmailInfrastructure;

/// <summary>
/// Provides functionality for sending emails.
/// </summary>
public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly IValidator<SendEmailCommand> _validator;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailService"/> class.
    /// </summary>
    /// <param name="configuration">The application configuration.</param>
    /// <param name="validator">The validator for the email command.</param>
    public EmailService(IConfiguration configuration, IValidator<SendEmailCommand> validator)
    {
        _configuration = configuration;
        _validator = validator;
    }

    /// <summary>
    /// Sends an email based on the provided command.
    /// </summary>
    /// <param name="command">The command containing the email details.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="CommandResult"/> representing the result of the email sending operation.
    /// </returns>
    public async Task<CommandResult> SendEmailAsync(SendEmailCommand command, CancellationToken cancellationToken)
    {
        Ensure.That(command).IsNotNull();

        ValidationResult validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errorMessages = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return CommandResult.Fail(errorMessages);
        }

        var emailSettings = _configuration.GetRequiredSection("EmailSettings").Get<EmailSettings>()
        ?? throw new InvalidOperationException("EmailSettings configuration is invalid");

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(command.Sender, emailSettings.SenderEmail));
        email.To.Add(new MailboxAddress(command.Receiver, command.ReceiverEmail));
        email.Subject = command.Subject;
        email.Body = new TextPart("plain")
        {
            Text = command.Body,
        };

        using (var smtpClient = new SmtpClient())
        {
            smtpClient.CheckCertificateRevocation = false;
            smtpClient.Connect(emailSettings.SmtpServer, emailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable);
            smtpClient.Authenticate(emailSettings.AuthLogin, emailSettings.Password);

            await smtpClient.SendAsync(email, cancellationToken);
            smtpClient.Disconnect(true);
        }

        return CommandResult.Success;
    }
}
