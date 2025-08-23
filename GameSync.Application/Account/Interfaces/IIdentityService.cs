using GameSync.Application.Account.UseCases.RegisterUser;
using GameSync.Domain.Shared.Commands;

namespace GameSync.Application.Account.Interfaces
{
    /// <summary>
    /// Provides identity-related operations such as user registration, authentication, and management.
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// Registers a new user in the system with the specified credentials and optional metadata.
        /// </summary>
        /// <param name="userName">The username to associate with the new user account.</param>
        /// <param name="email">The email address to associate with the new user account.</param>
        /// <param name="password">The password used to protect the user account.</param>
        /// <param name="lastIp">The IP address from which the registration request originated (optional).</param>
        /// <returns>
        /// A <see cref="RegisterResult"/> indicating the outcome of the registration operation, including success state and error information if applicable.
        /// </returns>
        Task<RegisterResult> RegisterAsync(string userName, string email, string password, string? lastIp);

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
        Task<CommandResult> ConfirmEmailAsync(string userId, string code);
    }
}
