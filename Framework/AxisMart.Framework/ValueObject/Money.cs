namespace AxisMart.Framework.ValueObject;

public sealed class Money : BaseValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        Guard.Against(amount < 0, $"{nameof(amount)}: Amount cannot be negative");
        Guard.AgainstNullOrEmpty(currency, nameof(currency));
        Guard.Against(currency.Length != 3, $"{nameof(currency)}: Currency must be a 3-letter ISO code");

        Amount = Math.Round(amount, 2);
        Currency = currency.ToUpperInvariant();
    }

    public static Money Zero(string currency) => new(0, currency);

    public Money Add(Money other)
    {
        Guard.Against(Currency != other.Currency, $"{nameof(other)} Cannot add {other.Currency} to {Currency}");

        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        Guard.Against(Currency != other.Currency, $"{nameof(other)}: Cannot subtract {other.Currency} from {Currency}");

        Guard.Against(Amount < other.Amount, $"{nameof(other)}: Insufficient amount");

        return new Money(Amount - other.Amount, Currency);
    }

    public Money Multiply(decimal factor)
    {
        Guard.Against(factor < 0, $"{nameof(factor)}: Factor cannot be negative");
        return new Money(Amount * factor, Currency);
    }

    public bool IsGreaterThan(Money other) =>
        Currency == other.Currency && Amount > other.Amount;

    public bool IsLessThan(Money other) =>
        Currency == other.Currency && Amount < other.Amount;

    public static Money operator +(Money left, Money right) => left.Add(right);
    public static Money operator -(Money left, Money right) => left.Subtract(right);
    public static Money operator *(Money left, decimal factor) => left.Multiply(factor);
    public static bool operator >(Money left, Money right) => left.IsGreaterThan(right);
    public static bool operator <(Money left, Money right) => left.IsLessThan(right);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString() => $"{Amount:F2} {Currency}";
}
