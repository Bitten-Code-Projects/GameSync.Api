namespace GameSync.Domain.Examples.Interfaces;

using GameSync.Domain.Examples.ValueObjects;
using GameSync.Domain.Shared.Interfaces;

public interface IExample : IAggregateRoot<long>
{
    string Name { get; }
    string Surname { get; }
    ExampleAddress Address { get; }
}
