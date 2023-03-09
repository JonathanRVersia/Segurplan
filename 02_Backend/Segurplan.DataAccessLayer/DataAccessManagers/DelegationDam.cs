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
    public class DelegationDam : SegurplanDamBase {
     
        public DelegationDam(SegurplanContext context, MemoryCacheService cacheService) : base(context, cacheService) {

        }

        public async Task<List<Delegation>> SelectAll() {
           
            try {

                List<Delegation> delegations = cacheService.TryGetValue<List<Delegation>>(CacheKeys.Delegations);
              
                if (delegations == default(List<Delegation>)) {

                    delegations = (from x in context.Delegation select x).ToList();
                    cacheService.SetValue(CacheKeys.Delegations, delegations);
                }

                return delegations;


                //return (from x in context.Delegation select x).ToList();
            } catch (Exception e) {
                Debug.WriteLine($"Error getting all delegations {e.ToString()}");

                return new List<Delegation>(0);
            }
        }        
    }
}
