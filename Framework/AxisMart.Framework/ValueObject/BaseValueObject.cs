namespace AxisMart.Framework.ValueObject;

public abstract class BaseValueObject : IEquatable<BaseValueObject>
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public bool Equals(BaseValueObject? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;

        return GetEqualityComponents()
            .SequenceEqual(other.GetEqualityComponents());
    }

    public override bool Equals(object? obj) =>
        obj is BaseValueObject other && Equals(other);

    public override int GetHashCode() =>
        GetEqualityComponents()
            .Aggregate(0, (hash, component) =>
                HashCode.Combine(hash, component?.GetHashCode() ?? 0));

    public static bool operator ==(BaseValueObject? left, BaseValueObject? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(BaseValueObject? left, BaseValueObject? right) =>
        !(left == right);
}
