using GameSync.Application.Account.Interfaces;
using MediatR;

namespace GameSync.Application.Account.UseCases.RegisterUser
{
    /// <summary>
    /// Handles the user registration command by delegating the operation to the identity service.
    /// </summary>
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, RegisterResult>
    {
        private readonly IIdentityService _identityService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterUserHandler"/> class.
        /// </summary>
        /// <param name="identityService">
        /// The <see cref="IIdentityService"/> used to perform user registration logic.
        /// </param>
        public RegisterUserHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        /// <summary>
        /// Handles the <see cref="RegisterUserCommand"/> by invoking the identity service
        /// to create a new user with the provided credentials and metadata.
        /// </summary>
        /// <param name="request">The registration command containing user details such as username, email, password, and last IP address.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>
        /// A <see cref="RegisterResult"/> indicating the success or failure of the registration attempt.
        /// </returns>
        public async Task<RegisterResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.RegisterAsync(request.UserName, request.Email, request.Password, request.LastIP);
            return result;
        }
    }
}
