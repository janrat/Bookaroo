using Microsoft.Extensions.Caching.Memory;
using System;

namespace Bookaroo.Services
{
    public interface ICacheService
    {
        T GetOrCreate<T>(string cacheKey, Func<T> createItem, MemoryCacheEntryOptions options);
        void Remove(string cacheKey);
        void InvalidateQueryCaches();
        void InvalidateCacheByPrefix(string prefix);
    }
}
