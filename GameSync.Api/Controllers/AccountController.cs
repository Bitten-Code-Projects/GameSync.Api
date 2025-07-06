using GameSync.Application.Account.Dtos;
using GameSync.Application.Account.UseCases.RegisterUser;
using GameSync.Infrastructure.Context.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameSync.Api.Controllers
{
    /// <summary>
    /// The AccountController handles all user account related operations.
    /// It provides endpoints for:
    /// - User registration,
    /// - User login with token issuance,
    /// - Retrieving the current user's profile information,
    /// - Changing user password,
    /// - Logging out the user.
    ///
    /// This controller is the central point for authentication and user management.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userManager">
        /// An instance of <see cref="UserManager{ApplicationUser}"/> used for managing application users.
        /// </param>
        /// <param name="logger">
        /// An instance of <see cref="ILogger{AccountController}"/> used for logging information, warnings, and errors (e.g., to Seq).
        /// </param>
        /// <param name="mediator">
        /// An instance of <see cref="IMediator"/> used to dispatch application-level commands and queries using the MediatR pattern.
        /// </param>
        public AccountController(UserManager<ApplicationUser> userManager, ILogger<AccountController> logger, IMediator mediator)
        {
            _userManager = userManager;
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Registers a new user with the provided credentials.
        /// </summary>
        /// <param name="dto">The registration data transfer object containing login, email, and password.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the HTTP response:
        /// <list type="bullet">
        /// <item><description>200 OK with a success message if registration succeeds.</description></item>
        /// <item><description>400 Bad Request with validation errors if the input is invalid or registration fails.</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Expects a JSON payload with the following structure:
        /// <code>
        /// {
        ///   "login": "string",
        ///   "email": "string",
        ///   "password": "string"
        /// }
        /// </code>
        /// </remarks>
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto, CancellationToken cancellationToken)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

            var safeEmail = dto.Email.Replace("\n", string.Empty).Replace("\r", string.Empty);
            var safeUserName = dto.Login.Replace("\n", string.Empty).Replace("\r", string.Empty);

            _logger.LogInformation(
                "[User Registration] Attempt: Email={Email}, IP={IP}, Username={Username}",
                safeEmail,
                ip,
                safeUserName);

            var command = new RegisterUserCommand
            {
                UserName = dto.Login,
                Password = dto.Password,
                Email = dto.Email,
                LastIP = ip,
            };

            var result = await _mediator.Send(command, cancellationToken);

            if (!result.Succeeded)
            {
                _logger.LogInformation(
                    "[User Registration] Registration failed for user: '{Username}' '{Email}'. Errors: {Errors}",
                    safeUserName,
                    safeEmail,
                    string.Join("; ", result.Errors.Select(e => e)));
                return BadRequest(result.Errors.Select(e => e));
            }

            // ToDo: Send email to activate account. (waiting for sending email feature)

            return Ok(new { message = "User registered successfully." });
        }
    }
}
