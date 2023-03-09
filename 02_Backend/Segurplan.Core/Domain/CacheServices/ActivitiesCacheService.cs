using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Segurplan.Core.Actions.Plans.PlansData.Activities;
using Segurplan.Core.Database;
using Segurplan.Core.Extensions.Comparers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Segurplan.Core.Domain.CacheServices {
    public class ActivitiesCacheService {
        private readonly IMemoryCache cache;
        private readonly IMapper mapper;
        private readonly SegurplanContext context;
        private IOptionsMonitor<CacheOptions> cacheOptions;

        public ActivitiesCacheService(IMemoryCache cache, IMapper mapper, SegurplanContext context, IOptionsMonitor<CacheOptions> cacheOptions) {
            this.cache = cache;
            this.mapper = mapper;
            this.context = context;
            this.cacheOptions = cacheOptions;
        }

        public async Task<IEnumerable<PlanChapter>> Get() {
            if (cache.TryGetValue<IEnumerable<PlanChapter>>(nameof(ActivitiesCacheService), out var chapters))
                return chapters;

            chapters = await context.ChapterVersion.Where(x => x.ApprovementDate < DateTime.Now && x.EndDate == null || x.EndDate > DateTime.Now)
               .ProjectTo<PlanChapter>(mapper.ConfigurationProvider)
              .ToListAsync();

            chapters = chapters.OrderBy(x => x.Number).ToList();                      

            if (!chapters.Any())
                return Enumerable.Empty<PlanChapter>();

            var cacheConfiguration = cacheOptions.Get(nameof(ActivitiesCacheService));

            cache.Set(nameof(ActivitiesCacheService), chapters, new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(cacheConfiguration.SlidingExpiration)
                  .SetAbsoluteExpiration(cacheConfiguration.AbsoluteExpiration));

            return chapters;
        }

        public void Invalidate() => cache.Remove(nameof(ActivitiesCacheService));

    }



}
