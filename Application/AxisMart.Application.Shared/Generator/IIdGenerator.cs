namespace AxisMart.Application.Shared.Generator;

public interface IIdGenerator
{
    Task<string> GenerateSerial();
    Task<string> GenerateRandomPassword();
}
