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

    public async Task<Example> CreateExample(Example example, CancellationToken cancellationToken = default)
    {
        var result = await _exampleDbContext.Example.AddAsync(example, cancellationToken);
        await _exampleDbContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<Example?> GetExampleById(long id, CancellationToken cancellationToken = default)
    {
        return await _exampleDbContext.Example.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}
