using GameSync.Application.Account.Dtos;
using GameSync.Infrastructure.Context.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameSync.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userManager">The <see cref="UserManager{ApplicationUser}"/> instance used to manage users.</param>
        /// <param name="logger">The <see cref="ILogger"/> instance used to send log entries to Seq.</param>
        public AccountController(UserManager<ApplicationUser> userManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _logger = logger;
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
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = dto.Login,
                Email = dto.Email,
            };

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
    }
}
