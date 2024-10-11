using MediatR;

namespace GameSync.Application.Examples.UseCases.GetExampleById;

public class GetExampleByIdQuery : IRequest<GetExampleByIdResult>
{
    public long Id { get; set; }
}
