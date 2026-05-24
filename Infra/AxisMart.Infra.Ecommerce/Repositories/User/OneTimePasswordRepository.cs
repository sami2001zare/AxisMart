using AxisMart.Core.Ecommerce.User;
using AxisMart.Core.Ecommerce.User.Repositpry;
using AxisMart.Core.Ecommerce.User.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AxisMart.Infra.Ecommerce.Repositories.User;

internal sealed class OneTimePasswordRepository(AxisMartContext axisMartContext) : IOneTimePasswordRepository
{
    public async Task AddAsync(OneTimePassword otp, CancellationToken cancellationToken = default)
    {
        await axisMartContext.AddAsync(otp, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<OneTimePassword> otps, CancellationToken cancellationToken = default)
    {
        await axisMartContext.AddAsync(otps, cancellationToken);
    }

    public async Task<OneTimePassword?> GetLatestByPhoneAsync(Phone phone, CancellationToken cancellationToken = default)
    {
        var otp = await axisMartContext.OTPs.Where(i => i.EmailOrPhone == phone.Value && !i.IsExpired).ToListAsync(cancellationToken);
        return otp.LastOrDefault();
    }

    public async Task<IEnumerable<OneTimePassword>> GetLatestsByPhoneAsync(Phone phone, CancellationToken cancellationToken = default)
    {
        return axisMartContext.OTPs.Where(i => i.EmailOrPhone == phone.Value && !i.IsExpired);
    }
}
