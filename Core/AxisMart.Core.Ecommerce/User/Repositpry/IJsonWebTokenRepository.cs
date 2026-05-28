namespace AxisMart.Core.Ecommerce.User.Repositpry;

public interface IJsonWebTokenRepository
{
    Task<JsonWebToken?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<JsonWebToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);

    Task AddAsync(JsonWebToken jwt, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<JsonWebToken> jwts, CancellationToken cancellationToken = default);
}
