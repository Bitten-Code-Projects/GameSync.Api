using GameSync.Domain.Shared.Commands;
using MediatR;

namespace GameSync.Application.Examples.UseCases.CreateExample;

public class CreateExampleCommand : IRequest<CommandResult>
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Street { get; set; }
    public required string City { get; set; }
    public required string HouseNumber { get; set; }
}
