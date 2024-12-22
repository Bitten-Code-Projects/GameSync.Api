namespace GameSync.Api.Controllers;

using GameSync.Application.Examples.UseCases.CreateExample;
using GameSync.Application.Examples.UseCases.GetExampleById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller for handling Example-related operations.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ExampleController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ExampleController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExampleController"/> class.
    /// </summary>
    /// <param name="mediator">The mediator instance for handling requests.</param>
    public ExampleController(IMediator mediator, ILogger<ExampleController> logger)
    {
        _logger = logger;
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves an example by its ID.
    /// </summary>
    /// <param name="id">The ID of the example to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>An ActionResult containing the requested example.</returns>
    [HttpGet("{id}", Name = "GetExample")]
    public async Task<IActionResult> GetExampleById(long id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting example by id: {id}", id);
        var query = new GetExampleByIdQuery
        {
            Id = id,
        };

        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Creates a new example.
    /// </summary>
    /// <param name="command">The command containing the data to create the example.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>An ActionResult containing the created example's information.</returns>
    [HttpPost("create")]
    public async Task<IActionResult> CreateExample(CreateExampleCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return CreatedAtRoute("GetExample", routeValues: new { id = result.Data }, result);
    }
}
