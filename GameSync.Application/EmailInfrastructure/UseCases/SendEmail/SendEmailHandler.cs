namespace GameSync.Application.EmailInfrastructure.UseCases.SendEmail;

using EnsureThat;
using FluentValidation;
using GameSync.Domain.Shared.Commands;
using MailKit.Net.Smtp;
using MediatR;
using MimeKit;
using Microsoft.Extensions.Configuration;

/// <summary>
/// Handles the processing of the <see cref="SendEmailCommand"/> to send emails.
/// This class validates the input, prepares the email message, and sends it via SMTP.
/// </summary>
public class SendEmailHandler : IRequestHandler<SendEmailCommand, CommandResult>
{
    private readonly IValidator<SendEmailCommand> _validator;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="SendEmailHandler"/> class.
    /// </summary>
    /// <param name="validator">The validator for the <see cref="SendEmailCommand"/>.</param>
    /// <param name="configuration">The application configuration used to access email settings or other configurations.</param>
    /// <param name="exampleRepository">An example repository for potential future use.</param>
    public SendEmailHandler(IValidator<SendEmailCommand> validator, IConfiguration configuration)
    {
        _validator = validator;
        _configuration = configuration;
    }

    /// <summary>
    /// Handles the <see cref="SendEmailCommand"/> to send an email.
    /// Validates the command, creates an email message, and sends it using an SMTP client.
    /// </summary>
    /// <param name="command">The command containing the email details (sender, receiver, subject, and body).</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="CommandResult"/> indicating success or failure.</returns>
    /// <exception cref="ValidationException">Thrown when the command validation fails.</exception>
    /// <exception cref="ArgumentNullException">Thrown when the command is null.</exception>
    public async Task<CommandResult> Handle(SendEmailCommand command, CancellationToken cancellationToken)
    {
        Ensure.That(command).IsNotNull();

        await _validator.ValidateAndThrowAsync(command);

        try
        {
            var emailSettings = _configuration.GetSection("EmailSettings").Get<EmailSettings>();

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(command.Sender, emailSettings!.SenderEmail));
            email.To.Add(new MailboxAddress(command.Receiver, command.ReceiverEmail));
            email.Subject = command.Subject;
            email.Body = new TextPart("plain")
            {
                Text = command.Body,
            };

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Connect(emailSettings!.SmtpServer, emailSettings!.SmtpPort, MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable);
                smtpClient.Authenticate(emailSettings!.AuthLogin, emailSettings!.Password);

                smtpClient.Send(email);
                smtpClient.Disconnect(true);
            }
        }
        catch (Exception ex)
        {
            return CommandResult.Fail($"Error occurs when sending email: {ex.Message}");
        }

        return CommandResult.Success;
    }
}