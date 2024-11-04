using GameSync.Domain.Examples.Interfaces;
using GameSync.Domain.Examples.ValueObjects;

namespace GameSync.Domain.Examples.Entities;

/// <summary>
/// Example entity.
/// </summary>
public class Example : IExample
{
    /// <inheritdoc/>
    public required long Id { get; set; } = 0;

    /// <inheritdoc/>
    public required string Name { get; set; } = string.Empty;

    /// <inheritdoc/>
    public required string Surname { get; set; } = string.Empty;

    /// <inheritdoc/>
    public required ExampleAddress Address { get; set; } = ExampleAddress.Empty;
}
