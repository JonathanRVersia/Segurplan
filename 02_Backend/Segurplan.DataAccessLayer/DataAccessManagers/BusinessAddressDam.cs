using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.DataAccessManagers.DamBase;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MemoryCache;

namespace Segurplan.DataAccessLayer.DataAccessManagers {
    public class BusinessAddressDam : SegurplanDamBase {
       
        public BusinessAddressDam(SegurplanContext context, MemoryCacheService cacheService) : base(context, cacheService) {
        }

        public async Task<List<BusinessAddress>> SelectAll() {
           
            try {
           
                List<BusinessAddress> businessAddress = cacheService.TryGetValue<List<BusinessAddress>>(CacheKeys.BussinessAddresses);
            
                if (businessAddress == default(List<BusinessAddress>)) {

                    businessAddress = (from bu in context.BusinessAddress select new BusinessAddress { Id = bu.Id, Name = bu.Name }).ToList();
                    cacheService.SetValue(CacheKeys.BussinessAddresses, businessAddress);
                }

                return businessAddress;
            
            } catch (Exception exc) {
            
                Debug.WriteLine($"Error getting business addresses :: {exc.ToString()}");

                return new List<BusinessAddress>(0);
            }

        }

    }
}
