using System.Text.RegularExpressions;

namespace AxisMart.Core.Ecommerce.User.ValueObjects;

public sealed record FirstName(string Value);

public sealed record EmailOrPhone
{
    private static readonly Regex EmailRegex = new(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private static readonly Regex PhoneRegex = new(
        @"^\+?[1-9]\d{6,14}$",
        RegexOptions.Compiled);

    public string Value { get; }
    public ContactType Type { get; }

    public bool IsEmail => Type == ContactType.Email;
    public bool IsPhone => Type == ContactType.Phone;

    public EmailOrPhone(string Value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Value);

        var trimmed = Value.Trim();

        if (EmailRegex.IsMatch(trimmed))
        {
            this.Value = trimmed.ToLowerInvariant();
            Type = ContactType.Email;
        }
        else if (PhoneRegex.IsMatch(trimmed))
        {
            this.Value = trimmed;
            Type = ContactType.Phone;
        }
        else
        {
            throw new ArgumentException(
                $"'{Value}' is neither a valid email nor a valid phone number.",
                nameof(Value));
        }
    }

    public static bool TryCreate(string value, out EmailOrPhone? result)
    {
        try
        {
            result = new EmailOrPhone(value);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public TResult Match<TResult>(
        Func<string, TResult> onEmail,
        Func<string, TResult> onPhone) =>
        Type switch
        {
            ContactType.Email => onEmail(Value),
            ContactType.Phone => onPhone(Value),
            _ => throw new InvalidOperationException("Unknown contact type.")
        };

}

public enum ContactType
{
    Email,
    Phone
}