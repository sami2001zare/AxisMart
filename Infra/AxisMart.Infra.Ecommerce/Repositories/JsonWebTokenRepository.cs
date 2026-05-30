using AxisMart.Core.Ecommerce.User;
using AxisMart.Core.Ecommerce.User.Repositpry;
using Microsoft.EntityFrameworkCore;

namespace AxisMart.Infra.Ecommerce.Repositories
{
    internal sealed class JsonWebTokenRepository(AxisMartContext axisMartContext) : IJsonWebTokenRepository
    {
        public async Task AddAsync(JsonWebToken jwt, CancellationToken cancellationToken = default)
        {
            await axisMartContext.JsonWebTokens.AddAsync(jwt, cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<JsonWebToken> jwts, CancellationToken cancellationToken = default)
        {
            await axisMartContext.JsonWebTokens.AddRangeAsync(jwts, cancellationToken);
        }

        public async Task<JsonWebToken?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await axisMartContext.JsonWebTokens.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        }

        public async Task<JsonWebToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            return await axisMartContext.JsonWebTokens.FirstOrDefaultAsync(i => i.Token == token, cancellationToken);
        }
    }
}
