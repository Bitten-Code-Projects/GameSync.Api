using GameSync.Api.Domain.Examples.ValueObjects;
using GameSync.Api.Domain.Examples.ValueObjects;

namespace GameSync.Api.Application.Examples.UseCases.GetExampleById;

public class GetExampleByIdResult
{
    public required long Id { get; set; } = 0;

    public required string Name { get; set; } = string.Empty;

    public required string Surname { get; set; } = string.Empty;

    public required ExampleAddress Address { get; set; }
}
