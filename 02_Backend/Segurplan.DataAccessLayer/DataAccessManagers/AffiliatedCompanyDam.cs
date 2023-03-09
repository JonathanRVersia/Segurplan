using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.DataAccessManagers.DamBase;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MemoryCache;

namespace Segurplan.DataAccessLayer.DataAccessManagers {
    public class AffiliatedCompanyDam : SegurplanDamBase {

        public AffiliatedCompanyDam(SegurplanContext context, MemoryCacheService cacheService) : base(context, cacheService) {

        }

        public async Task<List<AffiliatedCompany>> SelectAll() {

            List<AffiliatedCompany> affiliatedCompanies = cacheService.TryGetValue<List<AffiliatedCompany>>(CacheKeys.AffiliatedCompanies);
           
            if (affiliatedCompanies == default(List<AffiliatedCompany>)) {

                affiliatedCompanies = (from x in context.AffiliatedCompany
                                       select new AffiliatedCompany { Id = x.Id, Name = x.Name }).ToList();
                cacheService.SetValue(CacheKeys.AffiliatedCompanies, affiliatedCompanies);
            }

            return affiliatedCompanies;

        }
    }
}
