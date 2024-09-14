using GameSync.Api.Application.Examples.Interfaces;
using GameSync.Api.Domain.Examples.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Infrastructure.Examples;

public class ExampleRepository : IExampleRepository
{
    private readonly ExampleDbContext _exampleDbContext;

    public ExampleRepository(ExampleDbContext exampleDbContext)
    {
        _exampleDbContext = exampleDbContext;
    }

    public async Task<Example?> GetExampleById(long id, CancellationToken cancellationToken = default)
    {
        return await _exampleDbContext.Example.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}
