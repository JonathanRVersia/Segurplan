using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.DataAccessManagers.DamBase;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.DataAccessLayer.DataAccessManagers {
    public class ProfileDam : SegurplanDamBase {
        public ProfileDam(SegurplanContext context) : base(context) {

        }

        public async Task<List<Profile>> SelectAll() => (from pr in context.Profile select pr).ToList();
    }
}
