using GameSync.Domain.Shared.ValueObjects;

namespace GameSync.Domain.Examples.ValueObjects;

public class ExampleAddress : ValueObject
{
    public static ExampleAddress Empty => new ExampleAddress()
    {
        Street = string.Empty,
        City = string.Empty,
        HouseNumber = string.Empty,
    };

    public required string Street { get; set; }

    public required string City { get; set; }

    public required string HouseNumber { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return HouseNumber;
    }
}
