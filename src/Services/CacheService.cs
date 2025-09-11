using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.Memory;

namespace JustBeeWeb.Services;

public class CacheService
{
    private readonly HybridCache _hybridCache;
    private readonly IMemoryCache _memoryCache; // Fallback for compatibility

    public CacheService(HybridCache hybridCache, IMemoryCache memoryCache)
    {
        _hybridCache = hybridCache;
        _memoryCache = memoryCache;
    }

    /// <summary>
    /// Get or set a value using HybridCache with stampede protection
    /// </summary>
    public async Task<T> GetOrSetAsync<T>(string key, Func<CancellationToken, ValueTask<T>> factory,
        TimeSpan? expiry = null, CancellationToken stoppingToken = default)
    {
        var options = new HybridCacheEntryOptions
        {
            Expiration = expiry ?? TimeSpan.FromMinutes(5),
            LocalCacheExpiration = TimeSpan.FromMinutes(2)
        };

        return await _hybridCache.GetOrCreateAsync(key, factory, options, cancellationToken: stoppingToken);
    }

    /// <summary>
    /// Get or set a value with custom options
    /// </summary>
    public async Task<T> GetOrSetAsync<T>(string key, Func<CancellationToken, ValueTask<T>> factory,
        HybridCacheEntryOptions options, CancellationToken stoppingToken = default)
    {
        return await _hybridCache.GetOrCreateAsync(key, factory, options, cancellationToken: stoppingToken);
    }

    /// <summary>
    /// Synchronous version for backward compatibility
    /// </summary>
    public T? GetOrSet<T>(string key, Func<T> getItem, TimeSpan? expiry = null)
    {
        // For synchronous operations, fall back to memory cache
        if (_memoryCache.TryGetValue(key, out T? cachedValue))
        {
            return cachedValue;
        }

        var item = getItem();
        var actualExpiry = expiry ?? TimeSpan.FromMinutes(5);

        _memoryCache.Set(key, item, actualExpiry);
        return item;
    }

    /// <summary>
    /// Remove a specific cache entry
    /// </summary>
    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _hybridCache.RemoveAsync(key, cancellationToken);
        _memoryCache.Remove(key); // Also remove from memory cache
    }

    /// <summary>
    /// Remove multiple cache entries by pattern (for migration compatibility)
    /// </summary>
    public async Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default)
    {
        // For now, implement pattern-based removal with known keys
        // In future versions of HybridCache, this might be supported natively

        var keysToRemove = GetCacheKeysContaining(pattern);
        foreach (var key in keysToRemove)
        {
            await RemoveAsync(key, cancellationToken);
        }
    }

    /// <summary>
    /// Synchronous remove for backward compatibility
    /// </summary>
    public void Remove(string key)
    {
        _memoryCache.Remove(key);
        // Note: HybridCache remove is async, so this only removes from memory cache
        // For full removal, use RemoveAsync
    }

    /// <summary>
    /// Legacy pattern matching removal - simplified for current HybridCache API
    /// </summary>
    public void RemoveByPattern(string pattern)
    {
        // Basic pattern matching for memory cache only
        if (_memoryCache is MemoryCache mc)
        {
            var field = typeof(MemoryCache).GetField("_coherentState",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (field?.GetValue(mc) is object coherentState)
            {
                var entriesProperty = coherentState.GetType()
                    .GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (entriesProperty?.GetValue(coherentState) is System.Collections.IDictionary entries)
                {
                    var keysToRemove = new List<object>();

                    foreach (System.Collections.DictionaryEntry entry in entries)
                    {
                        if (entry.Key.ToString()?.Contains(pattern) == true)
                        {
                            keysToRemove.Add(entry.Key);
                        }
                    }

                    foreach (var key in keysToRemove)
                    {
                        _memoryCache.Remove(key);
                    }
                }
            }
        }
    }

    private List<string> GetCacheKeysContaining(string pattern)
    {
        // This is a simplified implementation
        // In a real scenario, you might want to maintain a list of cache keys
        var commonKeys = new[]
        {
            "MapBeeData",
            "AllDepartements",
            $"PersonsInDept_{pattern}"
        };

        return commonKeys.Where(key => key.Contains(pattern)).ToList();
    }
}