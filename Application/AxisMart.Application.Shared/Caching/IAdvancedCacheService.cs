namespace AxisMart.Application.Shared.Caching;

public interface IAdvancedCacheService
{
    Task<T?> GetStringAsync<T>(string key);

    Task SetStringAsync(
        string key,
        string value);

    Task SetStringAsync(
        string key,
        string value,
        TimeSpan? expiration = null);

    Task EvictStringAsync<T>(
        string key);

    Task RemoveAsync(string key);

    Task<bool> ExistsAsync(string key);
}