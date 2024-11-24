using GameSync.Domain.GameSync.Entities;

namespace GameSync.Domain.GameSync.Interfaces;

public interface IGameSyncRepository
{
    Task<LogEntity> CreateLogEntityAsync(LogEntity logEntity, CancellationToken cancellationToken = default);
}
