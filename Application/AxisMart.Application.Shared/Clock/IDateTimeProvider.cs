namespace AxisMart.Application.Shared.Clock;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
