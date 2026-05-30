using AxisMart.Application.Shared.Generator;

namespace AxisMart.Infra.Ecommerce.Clock;

internal sealed class IdGenerator : IIdGenerator
{
    public async Task<string> GenerateRandomPassword()
    {
        return "234";
    }

    public Task<string> GenerateSerial()
    {
        throw new NotImplementedException();
    }
}