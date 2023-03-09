using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.DataAccessManagers;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.Core.BusinessManagers {

    public class ApplicationUserManager {

        private const int NotAssignedRoleId = 99;
        private const string NotAssignedRoleName = "N/A";

        private readonly UserRolesDam usrRolDam;
        private readonly UserDam usrDam;
        private readonly RoleDam rolDam;
        private readonly UserManager<User> userManager;

        public ApplicationUserManager(UserRolesDam usrRolDam, UserDam usrDam, RoleDam rolDam, UserManager<User> userManager) {
            this.usrDam = usrDam;
            this.usrRolDam = usrRolDam;
            this.rolDam = rolDam;
            this.userManager = userManager;
        }

        public List<ApplicationRole> RoleList() {
            var result = new List<ApplicationRole>();

            var roleList = rolDam.SelectAll();

            result.Add(new ApplicationRole() {
                Id = NotAssignedRoleId,
                Name = NotAssignedRoleName
            });

            foreach (var dbRole in roleList) {
                result.Add(new ApplicationRole {
                    Id = Convert.ToInt32(dbRole.Id),
                    Name = dbRole.Name
                });
            }
            return result;
        }

        public List<ApplicationCenter> CenterList() {
            var response = new List<ApplicationCenter>();
            for (var i = 0; i < 3; i++) {
                var ctr = new ApplicationCenter();
                switch (i) {
                    case 0:
                        ctr.Id = 1;
                        ctr.Name = "Central";
                        break;
                    case 1:
                        ctr.Id = 2;
                        ctr.Name = "Norte";
                        break;
                    case 2:
                        ctr.Id = 3;
                        ctr.Name = "Sur";
                        break;
                    default:
                        break;
                }
                response.Add(ctr);
            }
            return response;
        }

        private User CheckDuplicated(int userId, string name, string userName, string userEmail) {

            if (userId != 0) {
                return usrDam.CheckUniqueFields(userId, userName, userEmail).Result;
            } else {
                // we are creating a user, so we dont care about id :-)
                return usrDam.CheckUniqueFields(userName, userEmail).Result;
            }
        }

        public async Task<ApplicationUser> CreateUser(ApplicationUser user) {
            try {
                // TODO: Add transactions!
                var responseUser = new ApplicationUser();
                responseUser = CheckUniqueFields(user);
                if (responseUser.IsUserNameOk && responseUser.IsNameOk && responseUser.IsEmailOk) {

                    var databaseUser = new User {
                        UserName = user.UserName,
                        NormalizedUserName = user.UserName.ToUpper(),
                        CompleteName = user.CompleteName,
                        Email = user.Email,
                        NormalizedEmail = user.Email.ToUpper(),
                        SecurityStamp = StampGenerator(),
                        CreateDate = DateTime.Now
                    };

                    var userId = await usrDam.CreateUserAsync(databaseUser);

                    if (userId > 0 && responseUser.UserRoleId != NotAssignedRoleId) {
                        responseUser.Id = userId;
                        //we need role from id because when creating users rolename always "N/A"
                        user.UserRoleName = usrRolDam.RoleNameFromId(user.UserRoleId);
                        //a rol must be asigned
                        var roleAsingOk = await userManager.AddToRoleAsync(databaseUser, user.UserRoleName);
                        if (roleAsingOk.Succeeded) {
                            return responseUser;
                        } else {
                            //Rolllback
                            if (await usrDam.DeleteUserAsync(databaseUser.Id)) {
                                responseUser.Id = -1;
                            } else {
                                throw new Exception("rollback error!");
                            }
                        }
                    }
                }
                responseUser.Id = -1;
                return responseUser;

            } catch (Exception) {
                return null;
            }
        }

        private ApplicationUser CheckUniqueFields(ApplicationUser user) {
            //email,name,username
            var response = CheckDuplicated(user.Id, user.CompleteName, user.UserName, user.Email);

            if(response == null) {
                user.IsEmailOk = true;
                user.IsNameOk = true;
                user.IsUserNameOk = true;
            } else {
                user.IsEmailOk = string.IsNullOrEmpty(response.Email);
                user.IsNameOk = string.IsNullOrEmpty(response.CompleteName);
                user.IsUserNameOk = string.IsNullOrEmpty(response.UserName);
            }
           
            return user;
        }

        public async Task<bool> AsingRoleAsync(User dbUser, int roleId) {
            //identity works with role name (not id) so we need to get name from id
            var roleName = usrRolDam.RoleNameFromId(roleId);
            var roleAsingOk = await userManager.AddToRoleAsync(dbUser, roleName);

            return roleAsingOk.Succeeded ? true : false;
        }

        public async Task<bool> UpdateUserRole(User dbUser, int roleId) {
            var currentRoles = await userManager.GetRolesAsync(dbUser);
            if (currentRoles.Count > 0) {//ALREADY GOT ROLE
                if (roleId != NotAssignedRoleId) {//DELETE AND UPDATE
                    var deleteRolOk = await userManager.RemoveFromRolesAsync(dbUser, currentRoles);
                    return deleteRolOk.Succeeded
                        ? await AsingRoleAsync(dbUser, roleId)
                        : false;
                } else {//JUST DELETE
                    var deleteRolOk = await userManager.RemoveFromRolesAsync(dbUser, currentRoles);
                    return deleteRolOk.Succeeded;
                }
            } else {//UNASIGNED ROLE
                return roleId == NotAssignedRoleId
                    ? true
                    : await AsingRoleAsync(dbUser, roleId);
            }
        }

        public async Task<ApplicationUser> UpdateUser(ApplicationUser user) {
            try {
                // TODO: Add transactions!
                var dbUser = usrDam.SelectUserById(user.Id);
                var rollBackUser = dbUser;
                if (dbUser == null) {
                    throw new Exception("User not found");
                }

                dbUser.ModifiedDate = DateTime.Now;
                dbUser.SecurityStamp = StampGenerator();
                var updateUserOk = await usrDam.UpdateUser(dbUser);

                if (updateUserOk) {
                    var updateRoleOk = await UpdateUserRole(dbUser, user.UserRoleId);
                    if (!updateRoleOk)
                        _ = await usrDam.UpdateUser(rollBackUser);
                }
                return new ApplicationUser();
            } catch (Exception e) {
                return null;
            }
        }

        public ApplicationUser UserData(int userId) {
            try {
                return ToApplicationUser(usrRolDam.SelectByUserId(userId));
            } catch (Exception) {
                return null;
            }
        }

        public async Task<bool> DeleteUser(int userId) {
            try {
                return await usrDam.DeleteUserAsync(userId);
            } catch (Exception e) {
                return false;
            }
        }

        private ApplicationUser ToApplicationUser(UserRole ray) {
            try {
                return new ApplicationUser() {
                    Id = ray.UserId,
                    CompleteName = ray.CompleteName,
                    UserName = ray.UserName,
                    UserRoleId = ray.RoleId > 0 ? ray.RoleId : NotAssignedRoleId,
                    UserRoleName = !string.IsNullOrEmpty(ray.UserRoleName) ? ray.UserRoleName : NotAssignedRoleName,
                    Email = ray.Email,
                    ModifiedDate = ray.ModifiedDate,
                    CreateDate = ray.CreateDate
                };
            } catch (Exception e) {
                return null;
            }
        }

        public static string StampGenerator() {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 32)
              .Select(s => s[random.Next(s.Length)]).ToArray().ToString());
        }
    }
}





