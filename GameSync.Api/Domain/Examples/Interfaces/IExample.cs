namespace GameSync.Api.Domain.Examples.Interfaces;

using GameSync.Api.Domain.Examples.ValueObjects;
using GameSync.Api.Domain.Shared.Interfaces;

public interface IExample : IAggregateRoot<long>
{
    string Name { get; }
    string Surname { get; }
    ExampleAddress Address { get; }
}
