using GameSync.Domain.GameSync.Entities;
using GameSync.Domain.GameSync.Interfaces;

namespace GameSync.Infrastructure.GameSync;

public class GameSyncRepository : IGameSyncRepository
{
    private readonly GameSyncDbContext _dbContext;

    public GameSyncRepository(GameSyncDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<LogEntity> CreateLogEntityAsync(LogEntity logEntity, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.AddAsync(logEntity, cancellationToken);
        await _dbContext.SaveChangesAsync();

        return result.Entity;
    }
}
