using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Segurplan.FrameworkExtensions.MemoryCache {
    public class MemoryCacheOptions {

        public MemoryCacheEntryOptions GetCacheEntryOptions(double silidingTime, double absoluteTime) {

            return new MemoryCacheEntryOptions()
                  .SetSlidingExpiration(TimeSpan.FromHours(silidingTime))
                  .SetAbsoluteExpiration(TimeSpan.FromHours(absoluteTime));

        }
    }
}
