using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.DataAccessManagers.DamBase;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.DataAccessLayer.DataAccessManagers {
    public class CustomerDam : SegurplanDamBase {
        public CustomerDam(SegurplanContext context) : base(context) {
        }

        public async Task<List<Customer>> SelectAll() {
            try {
                return (from x in context.Customer select x).ToList();
            } catch (Exception e) {
                Debug.WriteLine($"Error getting all delegations {e.ToString()}");

                return new List<Customer>(0);
            }
        }
    }
}
