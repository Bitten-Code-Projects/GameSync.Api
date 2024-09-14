namespace GameSync.Api.Domain.Shared.Interfaces;

public interface IAggregateRoot<TId>
{
    TId Id { get; }
}
