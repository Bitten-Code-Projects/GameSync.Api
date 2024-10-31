using GameSync.Domain.Shared.Commands;
using MediatR;

namespace GameSync.Application.Examples.UseCases.CreateExample;

/// <summary>
/// Represents a command to create an example entity.
/// This class implements IRequest<CommandResult> for use with MediatR.
/// </summary>
public class CreateExampleCommand : IRequest<CommandResult>
{
    /// <summary>
    /// Gets or sets the name of the example entity.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the surname of the example entity.
    /// </summary>
    public required string Surname { get; set; }

    /// <summary>
    /// Gets or sets the street name of the example entity's address.
    /// </summary>
    public required string Street { get; set; }

    /// <summary>
    /// Gets or sets the city name of the example entity's address.
    /// </summary>
    public required string City { get; set; }

    /// <summary>
    /// Gets or sets the house number of the example entity's address.
    /// </summary>
    public required string HouseNumber { get; set; }
}
