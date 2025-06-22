using System.Net;
using GameSync.Application.Account.ConfirmEmails;
using GameSync.Application.Account.Dtos;
using GameSync.Application.Shared.Commands;
using GameSync.Infrastructure.Context.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userManager">The <see cref="UserManager{ApplicationUser}"/> instance used to manage users.</param>
        /// <param name="logger">The <see cref="ILogger"/> instance used to send log entries to Seq.</param>
        /// <param name="mediator"> The <see cref="IMediator"/> instance used to send command and queries using CQRS pattern.</param>
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
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

            var user = new ApplicationUser
            {
                UserName = dto.Login,
                Email = dto.Email,
                LastIP = ip,
            };

            _logger.LogInformation(
                "[User Registration] Attempt: Email={Email}, IP={IP}, Username={Username}",
                dto.Email,
                ip,
                dto.Login);

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                _logger.LogWarning(
                    "Registration failed for user '{Login}' with email '{Email}'. Errors: {Errors}",
                    dto.Login,
                    dto.Email,
                    string.Join("; ", result.Errors.Select(e => e.Description)));
                return BadRequest(result.Errors.Select(e => e.Description));
            }

            // ToDo: Send email to activate account. (waiting for sending email feature)

            return Ok(new { message = "User registered successfully." });
        }

        /// <summary>
        /// Confirms user's email with provided confirmation code for specified user by user's id.
        /// </summary>
        /// <param name="userId">User's id.</param>
        /// <param name="code">Confirmation code.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the HTTP response:
        /// <list type="bullet">
        /// <item><description>200 OK with a success message if confirmation succeeds.</description></item>
        /// <item><description>400 Bad Request with validation error if the input is invalid or confirmation fails.</description></item>
        /// </list>
        /// </returns>
        [Authorize]
        [HttpGet("/confirm-email")]
        [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmEmail([FromRoute] Guid userId, [FromRoute] string code)
        {
            var command = new ConfirmEmailCommand(
                userId,
                code,
                HttpContext.User);

            var result = await _mediator.Send(command);
            if (result == CommandResult.Success)
            {
                return Ok();
            }

            _logger.LogWarning("Confirmation email failed - [{reason}]", result.FailureReason);
            return BadRequest(result.FailureReason);
        }
    }
}
