using AxisMart.Application.Shared.Clock;

namespace AxisMart.Infra.Ecommerce.Clock;

internal sealed class UtcDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
