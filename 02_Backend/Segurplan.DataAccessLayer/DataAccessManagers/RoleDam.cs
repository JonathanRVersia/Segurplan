using System.Collections.Generic;
using System.Linq;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.DataAccessLayer.DataAccessManagers {
    public class RoleDam {

        protected readonly SegurplanContext context;

        public RoleDam(SegurplanContext context) {
            this.context = context;
        }

        public List<Role> SelectAll() =>
            (
                from rol
                in context.Roles

                select new Role {
                    Id = rol.Id.ToString(),
                    Name = rol.Name

                }
                ).ToList();


    }
}
