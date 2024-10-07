using GameSync.Api.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Domain.Examples.ValueObjects;

public class ExampleAddress : ValueObject
{
    public required string Street { get; set; }

    public required string City { get; set; }

    public required string HouseNumber { get; set; }

    public static ExampleAddress Empty => new ExampleAddress()
    {
        Street = string.Empty,
        City = string.Empty,
        HouseNumber = string.Empty,
    };

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return HouseNumber;
    }
}
