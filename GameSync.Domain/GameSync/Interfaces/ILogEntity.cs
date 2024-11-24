using GameSync.Domain.Shared.Interfaces;

namespace GameSync.Domain.GameSync.Interfaces;

public interface ILogEntity : IAggregateRoot<long>
{
    string Severity { get; }

    string Data { get; }

    DateTime Date { get; }
}
