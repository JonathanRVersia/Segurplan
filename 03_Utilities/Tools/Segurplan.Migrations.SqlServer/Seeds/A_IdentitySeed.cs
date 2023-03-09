using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.FrameworkExtensions.Identity;
using Segurplan.Migrations.SqlServer.Helpers;
using Microsoft.EntityFrameworkCore;
using Segurplan.Migrations.SqlServer.Helpers.ActiveDirectory;
using Segurplan.Core.Helpers.ActiveDirectory;

namespace Segurplan.Migrations.SqlServer.Seeds {
    public class A_IdentitySeed : IDataSeed {

        private ActiveDirectoryOptions activeDirectoryOptions;
        private readonly UserManager<User> userManager;
        private readonly ActiveDirectoryService activeDirectoryService;
        private readonly ActiveDirectoryUserSeedHelper adHelper;

        public A_IdentitySeed(
            ActiveDirectoryOptions activeDirectoryOptions,
            UserManager<User> userManager,
            ActiveDirectoryService activeDirectoryService
            ) {
            this.activeDirectoryOptions = activeDirectoryOptions;
            this.userManager = userManager;
            this.activeDirectoryService = activeDirectoryService;
            this.adHelper = new ActiveDirectoryUserSeedHelper();
        }

        public async Task Seed(SegurplanContext context, CancellationToken cancellationToken = default) {

            //await AddOrUpdateInternalUsers();

            //common
            await AddRoles(context);

            //develop
            //await AddCoreTestDataUsers(context);
            //await AddUserRoles(context);
            //await AddUserLogins(context);


            //pre
            //await AddPreCoreTestDataUsers(context);
            //await AddPreUserRoles(context);
            //await AddPreUserLogins(context);
        }

        private async Task AddRoles(SegurplanContext context) {

            await context.AddRangeAsync(UsersData.Roles);
            context.Database.OpenConnection();
            context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Roles ON");
            await context.SaveChangesAsync();
            context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Roles OFF");
            context.Database.CloseConnection();
        }


        #region pre
        private async Task AddPreUserLogins(SegurplanContext context) {
            await context.AddRangeAsync(UsersData.PreUserLogins);
            await context.SaveChangesAsync();
        }
        private async Task AddPreUserRoles(SegurplanContext context) {
            await context.AddRangeAsync(UsersData.PreUserRoles);
            await context.SaveChangesAsync();
        }
        private async Task AddPreCoreTestDataUsers(SegurplanContext context) {
            await context.AddRangeAsync(UsersData.PreUsers);
            context.Database.OpenConnection();
            context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Users ON");
            await context.SaveChangesAsync();
            context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Users OFF");
            context.Database.CloseConnection();
        }
        #endregion


        #region develop
        private async Task AddUserRoles(SegurplanContext context) {
            await context.AddRangeAsync(UsersData.UserRoles);
            await context.SaveChangesAsync();
        }
        private async Task AddUserLogins(SegurplanContext context) {
            await context.AddRangeAsync(UsersData.UserLogins);
            await context.SaveChangesAsync();
        }
        private async Task AddCoreTestDataUsers(SegurplanContext context) {

            await context.AddRangeAsync(UsersData.Users);
            context.Database.OpenConnection();
            context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Users ON");
            await context.SaveChangesAsync();
            context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Users OFF");
            context.Database.CloseConnection();
        }
        #endregion

        private async Task AddOrUpdateInternalUsers() {

            string role;
            bool exists;
            List<ActiveDirectoryInfo> usersInfo = activeDirectoryService.GetActiveDirectoryUsers(adHelper.activeDirectoryUsers);

            foreach (var userInfo in usersInfo) {
                exists = false;

                var user = await userManager.FindByNameAsync(userInfo.UserName);

                if (user != null)
                    exists = true;
                else
                    user = new User();

                user.EmailConfirmed = true;
                user.PhoneNumberConfirmed = true;
                user.UserName = userInfo.UserName;
                user.NormalizedUserName = userInfo.UserName.ToUpper();
                user.SecurityStamp = Guid.NewGuid().ToString();
                user.Email = userInfo.UserEmail;
                user.NormalizedEmail = userInfo.UserEmail.ToUpper();

                var createResult = IdentityResult.Failed();

                if (exists)
                    createResult = await userManager.UpdateAsync(user);
                else {
                    createResult = await userManager.CreateAsync(user);
                    createResult.EnsureSuccess();

                    UserLoginInfo userLoginInfo = new UserLoginInfo("Active.Directory", userInfo.UserGuid, activeDirectoryOptions.ActiveDirectoryName);
                    createResult = await userManager.AddLoginAsync(user, userLoginInfo);
                    createResult.EnsureSuccess();
                }

                var roles = await userManager.GetRolesAsync(user);

                if (!roles.Any()) {
                    role = adHelper.activeDirectoryUsers.FirstOrDefault(x => x.Key.ToUpper().Equals(user.UserName.ToUpper())).Value;

                    var addToRoleResult = await userManager.AddToRoleAsync(user, role);

                    addToRoleResult.EnsureSuccess();
                }
            }
        }
    }
}
