namespace GameSync.Domain.Examples.Interfaces;

using global::GameSync.Domain.Examples.ValueObjects;
using global::GameSync.Domain.Shared.Interfaces;

/// <summary>
/// Represents an example entity in the GameSync domain.
/// </summary>
public interface IExample : IAggregateRoot<long>
{
    /// <summary>
    /// Gets the name of the example.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the surname of the example.
    /// </summary>
    string Surname { get; }

    /// <summary>
    /// Gets the address of the example.
    /// </summary>
    ExampleAddress Address { get; }
}