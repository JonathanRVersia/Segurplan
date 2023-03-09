using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Segurplan.Core.Actions.Administration.Users;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.DataAccessManagers;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.Core.BusinessManagers {

    public class ApplicationUserListManager {

        private readonly UserRolesDam usrDam;
        public int TotalUsers { get; private set; }

        private const string UserId = "userId";
        private const string Name = "name";
        private const string UserName = "userName";
        private const string UserRol = "userRol";
        public ApplicationUserListManager(UserRolesDam usrDam) {
            this.usrDam = usrDam;

        }

        public async Task<List<ApplicationUser>> ApplicationUserList(UsersListTableState tableState, UsersFilter tableFilter) {
            try {
                var userList = new List<UserRole>();
                var query = usrDam.GetAllUsersQuery();

                userList = await GetUserList(query, tableState, tableFilter);
                return ToApplicationUser(userList);
            } catch (Exception e) {
                return null;
            }
        }

        private void OrderQuery(string orderBy, string orderMode, ref IQueryable<UserRole> query) {

            switch (orderBy) {
                case UserId:
                    query = orderMode.Equals("desc") ? query.OrderByDescending(x => x.UserId) : query.OrderBy(x => x.UserId);
                    break;
                case Name:
                    query = orderMode.Equals("desc") ? query.OrderByDescending(x => x.CompleteName) : query.OrderBy(x => x.CompleteName);
                    break;
                case UserName:
                    query = orderMode.Equals("desc") ? query.OrderByDescending(x => x.UserName) : query.OrderBy(x => x.UserName);
                    break;
                case UserRol:
                    query = orderMode.Equals("desc") ? query.OrderByDescending(x => x.UserRoleName) : query.OrderBy(x => x.UserRoleName);
                    break;
                default:
                    query = orderMode.Equals("desc") ? query.OrderByDescending(x => x.UserId) : query.OrderBy(x => x.UserId);
                    break;
            }
        }

        private List<ApplicationUser> ToApplicationUser(List<UserRole> toConvert) {

            var response = new List<ApplicationUser>();
            foreach (var ray in toConvert) {

                var appUser = new ApplicationUser() {
                    Id = ray.UserId,
                    CompleteName = ray.CompleteName,
                    UserName = ray.UserName,
                    UserRoleName = ray.UserRoleName
                };
                response.Add(appUser);
            }
            return response;
        }

        private static void ApplyFilters(UsersFilter tableFilter, ref IQueryable<UserRole> query) {


            query = string.IsNullOrEmpty(tableFilter.Name)
                ? query
                : (from x in query where x.CompleteName.Contains(tableFilter.Name) select x);

            query = string.IsNullOrEmpty(tableFilter.UserName)
                ? query
                : (from x in query where x.UserName.Contains(tableFilter.UserName) select x);

            query = string.IsNullOrEmpty(tableFilter.UserRol)
                ? query
                : (from x in query where x.UserRoleName.Contains(tableFilter.UserRol) select x);
        }

        private async Task<List<UserRole>> GetUserList(IQueryable<UserRole> query, UsersListTableState tableState, UsersFilter tableFilter) {
            if (!string.IsNullOrEmpty(tableFilter.UserName)
                || !string.IsNullOrEmpty(tableFilter.Name)
                || !string.IsNullOrEmpty(tableFilter.UserRol)) {

                ApplyFilters(tableFilter, ref query);
            }

            TotalUsers = query.Count();

            OrderQuery(tableState.OrderBy, tableState.OrderMode, ref query);

            var skip = tableState.IndexPage * tableState.PageRows;

            if (query.Count() <= skip) {
                return query.ToList();
            } else {
                query = query.Skip(skip).Take(tableState.PageRows);
            }

            return query.ToList();
        }
    }
}





