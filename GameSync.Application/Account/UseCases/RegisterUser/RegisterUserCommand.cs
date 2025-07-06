using MediatR;

namespace GameSync.Application.Account.UseCases.RegisterUser
{
    /// <summary>
    /// Represents a command to register a new user in the system.
    /// </summary>
    public class RegisterUserCommand : IRequest<RegisterResult>
    {
        /// <summary>
        /// Gets or sets the username for the new user account.
        /// </summary>
        public required string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password for the new user account.
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// Gets or sets the email address for the new user account.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the IP address from which the registration was initiated.
        /// Optional, used for auditing or security purposes.
        /// </summary>
        public string? LastIP { get; set; }
    }
}
