using GameSync.Application.Account.Interfaces;
using GameSync.Application.Account.UseCases.RegisterUser;
using GameSync.Domain.Shared.Commands;
using GameSync.Infrastructure.Context.Models;
using Microsoft.AspNetCore.Identity;

namespace GameSync.Infrastructure.Identity
{
    /// <summary>
    /// Provides identity-related functionality such as registering users, implemented using ASP.NET Core Identity.
    /// </summary>
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityService"/> class.
        /// </summary>
        /// <param name="userManager">
        /// An instance of <see cref="UserManager{ApplicationUser}"/> used to manage user accounts and perform identity operations.
        /// </param>
        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Registers a new user with the given username, email, password, and optional IP address.
        /// </summary>
        /// <param name="userName">The username for the new account.</param>
        /// <param name="email">The email address for the new account.</param>
        /// <param name="password">The password to secure the new account.</param>
        /// <param name="lastIp">The IP address from which the registration was initiated (optional).</param>
        /// <returns>
        /// A <see cref="RegisterResult"/> indicating whether the registration was successful and including any error messages.
        /// </returns>
        public async Task<RegisterResult> RegisterAsync(string userName, string email, string password, string? lastIp)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = email,
                LastIP = lastIp,
            };

            var result = await _userManager.CreateAsync(user, password);

            return new RegisterResult
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors.Select(e => e.Description).ToArray(),
            };
        }

        /// <summary>
        /// Confirms a user's email address using the provided confirmation code.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose email should be confirmed.</param>
        /// <param name="code">The email confirmation code (token) returned by the identity system.</param>
        /// <returns>
        /// The result indicates whether the email was successfully confirmed.
        /// If the user cannot be found, a failed <see cref="CommandResult"/>
        /// is returned containing the message "User not found." or if the confirmation fails, a failed
        /// <see cref="CommandResult"/> is returned with the first error description from the identity result.
        /// </returns>
        public async Task<CommandResult> ConfirmEmailAsync(
            string userId,
            string code)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return CommandResult.Fail("User not found.");
            }

            var confirmationResult = await _userManager.ConfirmEmailAsync(user, code);

            return confirmationResult.Succeeded
                ? CommandResult.Success
                : CommandResult.Fail(confirmationResult.Errors.Select(e => e.Description).First());
        }
    }
}
