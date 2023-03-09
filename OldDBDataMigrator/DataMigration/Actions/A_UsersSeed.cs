using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OldDBDataMigrator.ProduccionDBModels;
using Segurplan.Core.Database;
using Segurplan.Core.Helpers.ActiveDirectory;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.FrameworkExtensions.Identity;

namespace OldDBDataMigrator.DataMigration.Actions {
    public class A_UsersSeed : ISeedInitializer {

        private readonly IMapper mapper;
        private readonly SegurplanContext segurplanContext;
        private readonly SegurplanProduccionContext produccionContext;
        private readonly UserManager<User> userManager;
        private readonly ActiveDirectoryService activeDirectoryService;
        private readonly ActiveDirectoryOptions activeDirectoryOptions;

        private List<Usuarios> usuarios;

        private List<ActiveDirectoryInfo> usersInfo;
        private List<User> users = new List<User>();

        private User oesiaUser = new User { UserName = "elecnor_dev", CompleteName = "Oesía", NormalizedUserName = "ELECNOR_DEV", Email = "elecnor_dev@oesia.com", NormalizedEmail = "ELECNOR_DEV@OESIA.COM", EmailConfirmed = true, PasswordHash = null, SecurityStamp = "EFMIBYZKVOVJS5Z7TQYBZMXA3V4U67GZ", ConcurrencyStamp = "024b36cf-4ace-422c-a2c9-706bbf5c1fda", PhoneNumber = null, PhoneNumberConfirmed = true, TwoFactorEnabled = false, LockoutEnd = null, LockoutEnabled = true, AccessFailedCount = 0 };
        private const string AdministradorRole = "Administrador";
        private const string UsuarioRole = "Usuario";

        //Desarrollo
        public Dictionary<string, string> activeDirectoryUsers = new Dictionary<string, string>() {

            {"IBILBAO","Administrador"},
            {"FAORTEGA","Administrador"},
            {"AGARCIAB","Administrador"},
            {"EGONZALEZL","Administrador"}
        };
        //Pre
        //public Dictionary<string, string> activeDirectoryUsers = new Dictionary<string, string>() {

        //    {"oesia","Administrador"},
        //    {"SVADILLO","Administrador"},
        //    {"VJAVIER","Administrador"},
        //    {"cglahuerta","Administrador"},
        //    {"SDEPEDRO","Administrador"}
        //};

        public A_UsersSeed(IMapper mapper, SegurplanContext segurplanContext, SegurplanProduccionContext produccionContext, UserManager<User> userManager, ActiveDirectoryService activeDirectoryService, ActiveDirectoryOptions activeDirectoryOptions) {
            this.mapper = mapper;
            this.segurplanContext = segurplanContext;
            this.produccionContext = produccionContext;
            this.userManager = userManager;
            this.activeDirectoryService = activeDirectoryService;
            this.activeDirectoryOptions = activeDirectoryOptions;
        }

        public async Task Seed() {
            //await GetOriginalData();
            //Convert();
            await SetDestinationData();
        }


        public async Task GetOriginalData() {
            usuarios = await produccionContext.Usuarios.ToListAsync();
            usersInfo = activeDirectoryService.GetActiveDirectoryUsers(activeDirectoryUsers);
        }

        public void Convert() {
            foreach (var usuario in usuarios) {
                usuario.Usuario = usuario.Usuario.Trim();

                var user = mapper.Map<User>(usuario);

                user.CreateDate = DateTime.Now;
                user.ModifiedDate = DateTime.Now;

                if (usersInfo != null) {
                    var userInfo = usersInfo.Where(x => x.UserName.ToUpper() == usuario.Usuario.ToUpper()).FirstOrDefault();

                    if (userInfo != null) {
                        if (!string.IsNullOrEmpty(userInfo.UserEmail)) {
                            user.Email = userInfo.UserEmail;
                            user.NormalizedEmail = userInfo.UserEmail.ToUpper();
                        }
                    }
                }
                users.Add(user);
            }
        }

        public async Task SetDestinationData() {
            //solo ejecutarlo cuando estemos en Desarrollo
            try {
                //Para desarrollo
                await AddOesiaUser();

                //AÑADIR USUARIO DE PRE OESIA EL PRIMERO

                //!!!!!!!!!!Dependiendo del provider key "elecnor o itdeusto" podemos saber que usuarios hay que coger/añadir

                //    si hay tiempo mirar que al compilar para publicar no haga las vistas dll https://stackoverflow.com/questions/40815493/net-core-mvc-deployment-publish-missing-views

                //foreach (var user in users) {

                //    var userInfo = usersInfo?.Where(x => x.UserName.ToUpper() == user.UserName.ToUpper()).FirstOrDefault();

                //    var createResult = await userManager.CreateAsync(user);
                //    createResult.EnsureSuccess();

                //    if (userInfo != null) {
                //        var userLoginInfo = new UserLoginInfo("Active.Directory", userInfo.UserGuid, activeDirectoryOptions.ActiveDirectoryName);
                //        createResult = await userManager.AddLoginAsync(user, userLoginInfo);
                //        createResult.EnsureSuccess();
                //    }

                //    var addToRoleResult = await userManager.AddToRoleAsync(user, UsuarioRole);
                //    addToRoleResult.EnsureSuccess();
                //}
            } catch (Exception ex) {

                throw ex;
            }

        }

        //Para Elecnor
        //private async Task AddOesiaUser() {

        //    var oesiaElecnorUser = users.Where(x => x.UserName == "oesia").FirstOrDefault();
        //    users.Remove(oesiaElecnorUser);

        //    if (!await segurplanContext.User.AnyAsync(x => x.NormalizedUserName == oesiaElecnorUser.UserName.ToUpper())) {

        //        var userInfo = usersInfo?.Where(x => x.UserName.ToUpper() == oesiaElecnorUser.UserName.ToUpper()).FirstOrDefault();

        //        var createResult = await userManager.CreateAsync(oesiaElecnorUser);
        //        createResult.EnsureSuccess();

        //        if (userInfo != null) {
        //            var userLoginInfo = new UserLoginInfo("Active.Directory", userInfo.UserGuid, activeDirectoryOptions.ActiveDirectoryName);
        //            createResult = await userManager.AddLoginAsync(oesiaElecnorUser, userLoginInfo);
        //            createResult.EnsureSuccess();
        //        }

        //        var addToRoleResult = await userManager.AddToRoleAsync(oesiaElecnorUser, UsuarioRole);
        //        addToRoleResult.EnsureSuccess();
        //    }

        //}

        //Para entorno desarrollo
        private async Task AddOesiaUser() {
            //User
            if (!await segurplanContext.User.AnyAsync(x => x.NormalizedUserName == oesiaUser.UserName.ToUpper())) {

                oesiaUser.CreateDate = DateTime.Now;
                oesiaUser.ModifiedDate = DateTime.Now;

                var createResult = await userManager.CreateAsync(oesiaUser);
                createResult.EnsureSuccess();

                var userLoginInfo = new UserLoginInfo("Active.Directory", "08b0eee1-11d7-40e5-b020-20d9447cd0a1", activeDirectoryOptions.ActiveDirectoryName);
                createResult = await userManager.AddLoginAsync(oesiaUser, userLoginInfo);
                createResult.EnsureSuccess();

                var addToRoleResult = await userManager.AddToRoleAsync(oesiaUser, AdministradorRole);
                addToRoleResult.EnsureSuccess();
            }

        }
    }
}
