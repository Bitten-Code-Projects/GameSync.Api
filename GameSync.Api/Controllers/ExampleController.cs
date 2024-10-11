namespace GameSync.Api.Controllers;

using GameSync.Application.Examples.UseCases.CreateExample;
using GameSync.Application.Examples.UseCases.GetExampleById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ExampleController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExampleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}", Name = "GetExample")]
    public async Task<IActionResult> GetExampleById(long id, CancellationToken cancellationToken)
    {
        var query = new GetExampleByIdQuery
        {
            Id = id,
        };

        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateExample(CreateExampleCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return CreatedAtRoute("GetExample", routeValues: new { id = result.Data }, result);
    }
}
