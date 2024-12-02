using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bookaroo.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private static readonly HashSet<string> _cacheKeys = new();

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T GetOrCreate<T>(string cacheKey, Func<T> createItem, MemoryCacheEntryOptions options)
        {
            if (!_cache.TryGetValue(cacheKey, out T cacheEntry))
            {
                cacheEntry = createItem();
                _cache.Set(cacheKey, cacheEntry, options);
                _cacheKeys.Add(cacheKey);
            }

            return cacheEntry;
        }

        public void Remove(string cacheKey)
        {
            _cache.Remove(cacheKey);
            _cacheKeys.Remove(cacheKey);
        }

        public void InvalidateQueryCaches()
        {
            foreach (var key in _cacheKeys)
            {
                _cache.Remove(key);
            }
            _cacheKeys.Clear();
        }

        public void InvalidateCacheByPrefix(string prefix)
        {
            var keysToRemove = _cacheKeys.Where(key => key.StartsWith(prefix)).ToList();
            foreach (var key in keysToRemove)
            {
                _cache.Remove(key);
                _cacheKeys.Remove(key);
            }
        }
    }
}
