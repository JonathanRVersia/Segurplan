using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Segurplan.FrameworkExtensions.MemoryCache {

    public class MemoryCacheService {

        private readonly IMemoryCache memoryCache;
        private readonly IConfiguration configuration;

        public MemoryCacheService(IMemoryCache memoryCache, IConfiguration configuration) {

            this.memoryCache = memoryCache;
            this.configuration = configuration;
        }

        public void SetValue(string key, object value) {

            MemoryCacheOptions options = new MemoryCacheOptions();
            double slidingExpiration = configuration.GetValue<double>("CacheExpiration:SlidingExpiration");
            double absoluteExpiration = configuration.GetValue<double>("CacheExpiration:AbsoluteExpiration");

            var cacheEntryOptions = options.GetCacheEntryOptions(slidingExpiration, absoluteExpiration);
            memoryCache.Set(key, value, cacheEntryOptions);
        }

        public T TryGetValue<T>(string key) {
            T entry;
            if (memoryCache.TryGetValue(key, out entry)) {
                return entry;
            } else {
                return default(T);
            }

        }

        public void Remove(string key) {
            memoryCache.Remove(key);
        }
    }
}
