using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.DataAccessManagers.DamBase;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.DataAccessLayer.DataAccessManagers {
    public class GeneralActivityDam : SegurplanDamBase {
        public GeneralActivityDam(SegurplanContext context) : base(context) {

        }

        public async Task<List<GeneralActivity>> SelectAll() {
            try {
                return (from ga in context.GeneralActivity select ga).ToList();
            } catch (Exception exc) {
                Debug.WriteLine($"Error getting general activities :: {exc.ToString()}");

                return new List<GeneralActivity>(0);
            }
        }
    }
}
