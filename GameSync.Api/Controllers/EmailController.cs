namespace GameSync.Api.Controllers;

using GameSync.Application.EmailInfrastructure.UseCases;
using MediatR;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller responsible for handling email-related operations.
/// Provides endpoints to send emails using the <see cref="SendEmailCommand"/>.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class EmailController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailController"/> class.
    /// </summary>
    /// <param name="mediator">The mediator instance used to handle commands and queries.</param>
    public EmailController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Sends an email based on the details provided in the <see cref="SendEmailCommand"/>.
    /// </summary>
    /// <param name="command">The command containing the email details, including sender, receiver, subject, and body.</param>
    /// <param name="cancellationToken">A token to cancel the operation if necessary.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the result of the operation:
    /// - Returns 200 OK if the email was sent successfully.
    /// - Returns 400 Bad Request if the operation failed.
    /// </returns>
    [HttpPost("send")]
    public async Task<IActionResult> SendEmail(SendEmailCommand command, CancellationToken cancellationToken)
    {

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}
