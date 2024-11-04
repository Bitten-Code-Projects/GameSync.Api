namespace GameSync.Domain.Shared.Interfaces;

/// <summary>
/// Aggregate root interface.
/// </summary>
/// <typeparam name="TId">Type of aggregate root.</typeparam>
public interface IAggregateRoot<TId>
{
    /// <summary>
    /// Gets id of aggregate root.
    /// </summary>
    TId Id { get; }
}
