using GameSync.Application.EmailInfrastructure;
using GameSync.Infrastructure.Context.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace GameSync.Infrastructure.Emails;

/// <summary>
///  Class responsible for sending emails to users by Identity module.
/// </summary>
/// <param name="emailService">Service responsible for sending emails.</param>
/// <param name="logger">Logger for logging.</param>
public class IdentityEmailSender(
    IEmailService emailService,
    ILogger<IdentityEmailSender> logger)
    : IEmailSender<ApplicationUser>
{
    /// <summary>
    /// Method responsible for sending email with confirmation link.
    /// </summary>
    /// <param name="user">User's data.</param>
    /// <param name="email">User's email address.</param>
    /// <param name="confirmationLink">Clickable link for confirming email address.</param>
    /// <returns>Awaitable `Task`.</returns>
    public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
    {
        logger.LogInformation("");
        var emailPayload = new SendEmailPayload
        {
            Sender = "GameSync",
            Subject = "Confirm your email",
            Body = $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.",
            Receiver = user.UserName ?? email,
            ReceiverEmail = email,
        };
        await emailService.SendEmailAsync(emailPayload, CancellationToken.None);
    }

    /// <summary>
    /// Method responsible for sending email with link that allows to reset user's password.
    /// </summary>
    /// <param name="user">User's data.</param>
    /// <param name="email">User's email address.</param>
    /// <param name="resetLink">Clickable link for resetting password.</param>
    /// <returns>Awaitable `Task`.</returns>
    public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
    {
        var emailPayload = new SendEmailPayload
        {
            Sender = "GameSync",
            Subject = "Reset your password",
            Body = $"Please reset your password by <a href='{resetLink}'>clicking here</a>.",
            Receiver = user?.UserName ?? email,
            ReceiverEmail = email,
        };
        await emailService.SendEmailAsync(emailPayload, CancellationToken.None);
    }

    /// <summary>
    /// Method responsible for sending email with reset code that allows to reset user's password.
    /// </summary>
    /// <param name="user">User's data.</param>
    /// <param name="email">User's email address.</param>
    /// <param name="resetCode">Password reset code.</param>
    /// <returns>Awaitable `Task`.</returns>
    public async Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
    {
        var emailPayload = new SendEmailPayload
        {
            Sender = "GameSync",
            Subject = "Reset your password",
            Body = "Please reset your password using the following code: " + resetCode,
            Receiver = user?.UserName ?? email,
            ReceiverEmail = email,
        };
        await emailService.SendEmailAsync(emailPayload, CancellationToken.None);
    }
}