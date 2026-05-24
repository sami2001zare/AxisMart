using System.Text.RegularExpressions;

namespace AxisMart.Framework;

public static class Guard
{
    public static void AgainstNull(object value, string parameterName)
    {
        if (value == null)
        {
            throw new ArgumentNullException(parameterName, $"{parameterName} cannot be null.");
        }
    }

    public static T AgainstNull<T>(T value, string parameterName) where T : class
    {
        if (value == null)
        {
            throw new ArgumentNullException(parameterName, $"{parameterName} cannot be null.");
        }
        return value;
    }
    public static void AgainstNullOrEmpty(string value, string parameterName)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException($"{parameterName} cannot be null or empty.", parameterName);
        }
    }

    public static void AgainstNullOrWhiteSpace(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{parameterName} cannot be null, empty, or whitespace.", parameterName);
        }
    }

    public static void AgainstNullOrEmpty<T>(IEnumerable<T> collection, string parameterName)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(parameterName, $"{parameterName} cannot be null.");
        }

        if (!collection.Any())
        {
            throw new ArgumentException($"{parameterName} cannot be empty.", parameterName);
        }
    }

    public static void AgainstLessThan<T>(T value, string parameterName, T min) where T : IComparable<T>
    {
        if (value.CompareTo(min) < 0)
        {
            throw new ArgumentOutOfRangeException(parameterName,
                $"{parameterName} cannot be less than {min}. Actual value: {value}");
        }
    }

    public static void AgainstLessThanOrEqualTo<T>(T value, string parameterName, T min) where T : IComparable<T>
    {
        if (value.CompareTo(min) <= 0)
        {
            throw new ArgumentOutOfRangeException(parameterName,
                $"{parameterName} cannot be less than or equal to {min}. Actual value: {value}");
        }
    }
    
    public static void AgainstGreaterThan<T>(T value, string parameterName, T max) where T : IComparable<T>
    {
        if (value.CompareTo(max) > 0)
        {
            throw new ArgumentOutOfRangeException(parameterName,
                $"{parameterName} cannot be greater than {max}. Actual value: {value}");
        }
    }

    public static void AgainstGreaterThanOrEqualTo<T>(T value, string parameterName, T max) where T : IComparable<T>
    {
        if (value.CompareTo(max) >= 0)
        {
            throw new ArgumentOutOfRangeException(parameterName,
                $"{parameterName} cannot be greater than or equal to {max}. Actual value: {value}");
        }
    }

    public static void AgainstOutOfRange<T>(T value, string parameterName, T min, T max) where T : IComparable<T>
    {
        if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
        {
            throw new ArgumentOutOfRangeException(parameterName,
                $"{parameterName} must be between {min} and {max} (inclusive). Actual value: {value}");
        }
    }

    public static void AgainstNegative(int value, string parameterName)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(parameterName,
                $"{parameterName} cannot be negative. Actual value: {value}");
        }
    }

    public static void AgainstNegative(decimal value, string parameterName)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(parameterName,
                $"{parameterName} cannot be negative. Actual value: {value}");
        }
    }

    public static void AgainstNegative(double value, string parameterName)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(parameterName,
                $"{parameterName} cannot be negative. Actual value: {value}");
        }
    }

    public static void AgainstExceedsLength(string value, string parameterName, int maxLength)
    {
        AgainstNullOrEmpty(value, parameterName);

        if (value.Length > maxLength)
        {
            throw new ArgumentException(
                $"{parameterName} cannot exceed {maxLength} characters. Actual length: {value.Length}",
                parameterName);
        }
    }
    
    public static void AgainstLengthOutOfRange(string value, string parameterName, int minLength, int maxLength)
    {
        AgainstNullOrEmpty(value, parameterName);

        if (value.Length < minLength || value.Length > maxLength)
        {
            throw new ArgumentException(
                $"{parameterName} must be between {minLength} and {maxLength} characters. Actual length: {value.Length}",
                parameterName);
        }
    }

    public static void AgainstInvalidFormat(string value, string parameterName, string pattern, string? message = null)
    {
        AgainstNullOrEmpty(value, parameterName);

        if (!Regex.IsMatch(value, pattern))
        {
            throw new ArgumentException(
                message ?? $"{parameterName} has an invalid format. Value: {value}",
                parameterName);
        }
    }

    public static void AgainstInvalidEmail(string value, string parameterName)
    {
        AgainstNullOrWhiteSpace(value, parameterName);

        // Simple email validation pattern
        const string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        if (!Regex.IsMatch(value, emailPattern))
        {
            throw new ArgumentException(
                $"{parameterName} is not a valid email address. Value: {value}",
                parameterName);
        }
    }

    public static void Against(bool condition, string message)
    {
        if (condition)
        {
            throw new InvalidOperationException(message);
        }
    }

    public static void AgainstArgument(bool condition, string parameterName, string message)
    {
        if (condition)
        {
            throw new ArgumentException(message, parameterName);
        }
    }

    public static void AgainstInvalid<T>(T value, string parameterName, Func<T, bool> predicate, string? message = null)
    {
        if (!predicate(value))
        {
            throw new ArgumentException(
                message ?? $"{parameterName} is invalid. Value: {value}",
                parameterName);
        }
    }

    public static void AgainstDefault<T>(T value, string parameterName)
    {
        if (EqualityComparer<T>.Default.Equals(value, default(T)))
        {
            throw new ArgumentException(
                $"{parameterName} cannot be the default value. Value: {value}",
                parameterName);
        }
    }

    public static void AgainstInvalidType(object value, string parameterName, Type expectedType)
    {
        AgainstNull(value, parameterName);

        if (value.GetType() != expectedType && !expectedType.IsAssignableFrom(value.GetType()))
        {
            throw new ArgumentException(
                $"{parameterName} must be of type {expectedType.Name}. Actual type: {value.GetType().Name}",
                parameterName);
        }
    }

    public static void AgainstInvalidState(bool isValid, string message)
    {
        if (!isValid)
        {
            throw new InvalidOperationException(message);
        }
    }

    public static void AgainstAlreadyInitialized(bool isInitialized, string objectName)
    {
        if (isInitialized)
        {
            throw new InvalidOperationException($"{objectName} has already been initialized.");
        }
    }

    public static void AgainstContainsNull<T>(IEnumerable<T> collection, string parameterName) where T : class
    {
        AgainstNull(collection, parameterName);

        if (collection.Any(item => item == null))
        {
            throw new ArgumentException($"{parameterName} cannot contain null elements.", parameterName);
        }
    }

    public static void AgainstContainsDuplicates<T>(IEnumerable<T> collection, string parameterName)
    {
        AgainstNull(collection, parameterName);

        var distinctCount = collection.Distinct().Count();
        if (distinctCount < collection.Count())
        {
            throw new ArgumentException($"{parameterName} cannot contain duplicate elements.", parameterName);
        }
    }

    public static void AgainstExceedsSize<T>(IEnumerable<T> collection, string parameterName, int maxSize)
    {
        AgainstNull(collection, parameterName);

        var count = collection.Count();
        if (count > maxSize)
        {
            throw new ArgumentException(
                $"{parameterName} cannot exceed {maxSize} elements. Actual count: {count}",
                parameterName);
        }
    }

    public static void AgainstPastDate(DateTime date, string parameterName)
    {
        if (date < DateTime.UtcNow)
        {
            throw new ArgumentException(
                $"{parameterName} cannot be in the past. Date: {date:yyyy-MM-dd}",
                parameterName);
        }
    }

    public static void AgainstFutureDate(DateTime date, string parameterName)
    {
        if (date > DateTime.UtcNow)
        {
            throw new ArgumentException(
                $"{parameterName} cannot be in the future. Date: {date:yyyy-MM-dd}",
                parameterName);
        }
    }

    public static void AgainstInvalidDateRange(DateTime startDate, DateTime endDate,
        string startParameterName, string endParameterName)
    {
        if (endDate < startDate)
        {
            throw new ArgumentException(
                $"{endParameterName} ({endDate:yyyy-MM-dd}) cannot be before {startParameterName} ({startDate:yyyy-MM-dd})",
                endParameterName);
        }
    }

    public class GuardBuilder<T>(T value, string parameterName)
    {
        private readonly List<string> _errors = [];

        public GuardBuilder<T> Must(Func<T, bool> condition, string errorMessage)
        {
            if (!condition(value))
            {
                _errors.Add(errorMessage);
            }
            return this;
        }

        public void Validate()
        {
            if (_errors.Any())
            {
                throw new ArgumentException(
                    $"{parameterName} validation failed: {string.Join("; ", _errors)}",
                    parameterName);
            }
        }
    }

    public static GuardBuilder<T> For<T>(T value, string parameterName)
    {
        return new GuardBuilder<T>(value, parameterName);
    }
}