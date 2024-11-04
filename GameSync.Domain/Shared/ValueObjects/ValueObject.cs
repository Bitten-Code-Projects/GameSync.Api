namespace GameSync.Domain.Shared.ValueObjects;

/// <summary>
/// Value object base class.
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Value object's equals.
    /// </summary>
    /// <param name="obj">Compared object.</param>
    /// <returns>Boolean result.</returns>
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <summary>
    /// Calculates hash code.
    /// </summary>
    /// <returns>Hash code.</returns>
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }

    /// <summary>
    /// Equal operator overload.
    /// </summary>
    /// <param name="left">Left object of comparison.</param>
    /// <param name="right">Right object of comparison.</param>
    /// <returns>Boolean result.</returns>
    protected static bool EqualOperator(ValueObject left, ValueObject right)
    {
        if (left is null ^ right is null)
        {
            return false;
        }

        return ReferenceEquals(left, right) || left!.Equals(right);
    }

    /// <summary>
    /// Not equal operator overload.
    /// </summary>
    /// <param name="left">Left object of comparison.</param>
    /// <param name="right">Right object of comparison.</param>
    /// <returns>Boolean result.</returns>
    protected static bool NotEqualOperator(ValueObject left, ValueObject right)
    {
        return !EqualOperator(left, right);
    }

    /// <summary>
    /// Abstract getter for equality components.
    /// </summary>
    /// <returns>Equality components in a collection.</returns>
    protected abstract IEnumerable<object> GetEqualityComponents();
}
