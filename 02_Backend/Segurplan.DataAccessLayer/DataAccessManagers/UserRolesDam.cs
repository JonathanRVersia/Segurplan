using System.Linq;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.DataAccessLayer.DataAccessManagers {
    public class UserRolesDam {

        protected readonly SegurplanContext context;


        public UserRolesDam(SegurplanContext context) {
            this.context = context;
        }

        public UserRole SelectByUserId(int userId) {
            return (from x in GetAllUsersQuery()
                    where x.UserId == userId
                    select x).FirstOrDefault();
        }

        public IQueryable<UserRole> GetAllUsersQuery() {
            return from u in context.Users
                    join ur in context.UserRoles on u.Id equals ur.UserId into applicationUsers
                    from appUsr in applicationUsers.DefaultIfEmpty()
                    join r in context.Roles on appUsr.RoleId equals r.Id into userRoles
                    from usrRoles in userRoles.DefaultIfEmpty()
                    select new UserRole {
                        UserId = u.Id,
                        RoleId = (int?)appUsr.RoleId != null ? appUsr.RoleId : 0,
                        NormalizedName = u.NormalizedUserName,
                        CompleteName = u.CompleteName,
                        UserName = u.UserName,
                        UserRoleName = usrRoles.Name != null ? usrRoles.Name : "",
                        Email = u.Email != null ? u.Email : "N/A",
                        ModifiedDate = u.ModifiedDate.ToString("dd/MM/yyyy HH:mm"),
                        CreateDate = u.CreateDate.ToString("dd/MM/yyyy HH:mm")
                    };
        }

        public string RoleNameFromId(int roleId) {
            return (from x in context.Roles
                    where x.Id == roleId
                    select x.Name).FirstOrDefault();
        }
    }
}
