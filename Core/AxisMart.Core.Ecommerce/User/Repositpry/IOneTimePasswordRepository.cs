using AxisMart.Core.Ecommerce.User.ValueObjects;

namespace AxisMart.Core.Ecommerce.User.Repositpry;

public interface IOneTimePasswordRepository
{
    Task<IEnumerable<OneTimePassword>> GetLatestsByPhoneAsync(Phone phone, CancellationToken cancellationToken = default);
    Task<OneTimePassword?> GetLatestByPhoneAsync(Phone phone, CancellationToken cancellationToken = default);

    Task AddAsync(OneTimePassword otp, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<OneTimePassword> otps, CancellationToken cancellationToken = default);
}
