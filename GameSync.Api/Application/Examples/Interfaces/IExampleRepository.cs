using GameSync.Api.Domain.Examples.Entities;

namespace GameSync.Api.Application.Examples.Interfaces;

public interface IExampleRepository
{
    Task<Example?> GetExampleById(long id, CancellationToken cancellationToken = default);
}
