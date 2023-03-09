using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.FrameworkExtensions.Identity;
using Segurplan.Migrations.SqlServer.Helpers.ActiveDirectory;

namespace Segurplan.Migrations.SqlServer.Seeds {
    public class IdentitySeed : IDataSeed {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole<int>> roleManager;
        private readonly ActiveDirectoryOptions activeDirectoryOptions;
        private readonly ActiveDirectoryService activeDirectoryService;
        private readonly string[] internalUserNames = { "agarciab", "ugoyenaga", "dmedinap", "faortega", "elecnor_dev" };

        public IdentitySeed(
            UserManager<User> userManager,
            RoleManager<IdentityRole<int>> roleManager,
             ActiveDirectoryService activeDirectoryService,
            ActiveDirectoryOptions activeDirectoryOptions) {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.activeDirectoryOptions = activeDirectoryOptions;
            this.activeDirectoryService = activeDirectoryService;
        }

        public async Task Seed(SegurplanContext context, CancellationToken cancellationToken = default) {
            await AddOrUpdateAdministratorRole();
            await AddOrUpdateUsuarioRole();
            await AddOrUpdateInternalUsers();
            await AddDataToInitializeInDatabase();
        }

        // Insertar Datos random para inicializar la BBDD
        private async Task AddDataToInitializeInDatabase() {
            using (var contexto = new SegurplanContext()) {
                var insertDatos = from table in contexto.Template
                                  select table;
            }
        }

        private async Task<IdentityRole<int>> AddOrUpdateAdministratorRole() {
            var exists = false;
            var role = await roleManager.FindByNameAsync("Administrador");

            if (role != null)
                exists = true;
            else
                role = new IdentityRole<int>();

            role.Name = "Administrador";

            var roleResult = IdentityResult.Failed();

            if (exists)
                roleResult = await roleManager.UpdateAsync(role);
            else
                roleResult = await roleManager.CreateAsync(role);

            roleResult.EnsureSuccess();

            return role;
        }

        private async Task<IdentityRole<int>> AddOrUpdateUsuarioRole() {
            var exists = false;
            var role = await roleManager.FindByNameAsync("Usuario");

            if (role != null)
                exists = true;
            else
                role = new IdentityRole<int>();

            role.Name = "Usuario";
            var roleResult = IdentityResult.Failed();

            if (exists)
                roleResult = await roleManager.UpdateAsync(role);
            else
                roleResult = await roleManager.CreateAsync(role);

            roleResult.EnsureSuccess();

            return role;
        }


        private async Task AddOrUpdateInternalUsers() {
            bool exists;
            var usersInfo = activeDirectoryService.GetActiveDirectoryUsers(internalUserNames);

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

                    var userLoginInfo = new UserLoginInfo("Active.Directory", userInfo.UserGuid, activeDirectoryOptions.ActiveDirectoryName);
                    createResult = await userManager.AddLoginAsync(user, userLoginInfo);
                    createResult.EnsureSuccess();
                }

                var roles = await userManager.GetRolesAsync(user);

                if (!roles.Any() && user.UserName != "elecnor_dev") {
                    var addToRoleResult = await userManager.AddToRoleAsync(user, "Administrador");
                    addToRoleResult.EnsureSuccess();

                } else if (!roles.Any() && user.UserName == "elecnor_dev") {
                    var addToRoleResult = await userManager.AddToRoleAsync(user, "Usuario");
                    addToRoleResult.EnsureSuccess();
                }
            }
        }

    }
}
