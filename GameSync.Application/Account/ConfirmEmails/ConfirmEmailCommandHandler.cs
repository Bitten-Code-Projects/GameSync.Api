using GameSync.Application.Shared.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GameSync.Application.Account.ConfirmEmails;

/// <summary>
/// Command handler class used to confirm user's email with confirmation code
/// </summary>
public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, CommandResult>
{
    private readonly UserManager<IdentityUser> _userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfirmEmailCommandHandler"/> class.
    /// </summary>
    /// <param name="userManager">User manager instance.</param>
    public ConfirmEmailCommandHandler(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    /// <summary>
    /// Handle method responsible for user's email confirmation.
    /// </summary>
    /// <param name="command">Command to execute.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Command result.</returns>
    public async Task<CommandResult> Handle(ConfirmEmailCommand command, CancellationToken cancellationToken)
    {
        var user = await _userManager.GetUserAsync(command.Claims);

        if (user is null)
        {
            return CommandResult.Fail("User wasn't found.");    // TODO: What error to return?
        }

        if (user.Id != command.UserId.ToString())
        {
            return CommandResult.Fail("User's id doesn't match.");  // TODO: What error to return?
        }

        var confirmationResult = await _userManager.ConfirmEmailAsync(user, command.Code);

        if (confirmationResult.Succeeded)
        {
            return CommandResult.Success;
        }

        string errors = string.Empty;
        confirmationResult.Errors.ToList().ForEach(error => errors += error.Description + Environment.NewLine);
        return CommandResult.Fail(errors);  // TODO: CommandResult is missing `IEnumerable<string>` reasons list
    }
}