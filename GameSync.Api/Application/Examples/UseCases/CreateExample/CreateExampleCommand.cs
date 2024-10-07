using GameSync.Api.Application.Examples.UseCases.GetExampleById;
using GameSync.Api.Domain.Shared.Commands;
using MediatR;

namespace GameSync.Api.Application.Examples.UseCases.CreateExample;

public class CreateExampleCommand : IRequest<CommandResult>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string HouseNumber { get; set; }
}
