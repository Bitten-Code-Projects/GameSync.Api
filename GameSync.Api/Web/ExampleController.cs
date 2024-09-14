namespace GameSync.Api.Web;

using GameSync.Api.Application.Examples.UseCases.GetExampleById;
using MediatR;
using Microsoft.AspNetCore.Http;
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetExampleById(long id, CancellationToken cancellationToken)
    {
        var query = new GetExampleByIdQuery
        {
            Id = id,
        };

        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}
