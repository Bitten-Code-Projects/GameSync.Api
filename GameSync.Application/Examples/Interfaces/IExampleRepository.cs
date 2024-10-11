using GameSync.Domain.Examples.Entities;

namespace GameSync.Application.Examples.Interfaces;

public interface IExampleRepository
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    Task<Example?> GetExampleById(long id, CancellationToken cancellationToken = default);

    Task<Example> CreateExample(Example example, CancellationToken cancellationToken = default);
}
