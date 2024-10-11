using GameSync.Domain.Examples.Interfaces;
using GameSync.Domain.Examples.ValueObjects;

namespace GameSync.Domain.Examples.Entities;

public class Example : IExample
{
    public required long Id { get; set; } = 0;

    public required string Name { get; set; } = string.Empty;

    public required string Surname { get; set; } = string.Empty;

    public required ExampleAddress Address { get; set; } = ExampleAddress.Empty;
}
