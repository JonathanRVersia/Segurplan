using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Segurplan.Core.Actions.Administration.Users.Details;
using Segurplan.Core.Database;
using Segurplan.Core.Helpers.ActiveDirectory;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.FrameworkExtensions.Identity;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Users.UpdateUser {

    public class UpdateUserRequestHandler : IRequestHandler<UpdateUserRequest, IRequestResponse<UserDetailsResponse>> {

        private readonly UserManager<User> userManager;
        private readonly ActiveDirectoryService activeDirectoryService;
        private readonly ActiveDirectoryOptions activeDirectoryOptions;
        private readonly SegurplanContext context;

        public UpdateUserRequestHandler(UserManager<User> userManager, ActiveDirectoryService activeDirectoryService, ActiveDirectoryOptions activeDirectoryOptions, SegurplanContext context) {
            this.userManager = userManager;
            this.activeDirectoryService = activeDirectoryService;
            this.activeDirectoryOptions = activeDirectoryOptions;
            this.context = context;
        }

        public async Task<IRequestResponse<UserDetailsResponse>> Handle(UpdateUserRequest request, CancellationToken cancellationToken) {

            User user;

            if (request.Id == 0)
                user = await CreateUser(request);
            else
                user = await UpdateUser(request);
            var userInfo = activeDirectoryService.GetActiveDirectoryUser(user.UserName);
            return RequestResponse.Ok(new UserDetailsResponse {
                Id = user.Id,
                CompleteName = user.CompleteName,
                CreateDate = user.CreateDate,
                ModifiedDate = user.ModifiedDate,
                Email = user.Email,
                IsSuscribed = userInfo == null ? false : request.IsSuscribed,
                UserName = user.UserName,
                UserRole = request.UserRole
            });
        }

        private async Task<User> CreateUser(UpdateUserRequest request) {

            var user =  new User {
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                UserName = request.UserName,
                NormalizedUserName = request.UserName.ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
                Email = request.Email,
                NormalizedEmail = request.Email != null ? request.Email.ToUpper() : string.Empty,
                CompleteName = request.CompleteName,
                CreateDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            if(await AddUser(user)) {
                var identityResult = await userManager.AddToRoleAsync(user, request.UserRole);
                identityResult.EnsureSuccess();

                if (request.IsSuscribed)
                    await AddUserLogin(user, request.UserADGuid);
            }

            return user;
        }

        private async Task<User> UpdateUser(UpdateUserRequest request) {

            var user = await userManager.FindByIdAsync(request.Id.ToString());

            user.ModifiedDate = DateTime.Now;

            var userLogins = await userManager.GetLoginsAsync(user);

            if (userLogins.Any() && !request.IsSuscribed)
                await RemoveUserLogin(user);
            else if(!userLogins.Any() && request.IsSuscribed) {
                var userInfo = activeDirectoryService.GetActiveDirectoryUser(user.UserName);
                if(userInfo != null) {
                    await AddUserLogin(user, userInfo.UserGuid);
                }
            }

            var identityResult = await userManager.RemoveFromRolesAsync(user, await userManager.GetRolesAsync(user));
            identityResult.EnsureSuccess();

            identityResult = await userManager.AddToRoleAsync(user, request.UserRole);
            identityResult.EnsureSuccess();

            user.ModifiedDate = DateTime.Now;
            identityResult = await userManager.UpdateAsync(user);
            identityResult.EnsureSuccess();

            return user;
        }

        private async Task<bool> RemoveUserLogin(User user) {
            bool response = false;

            var userGuid = context.UserLogins.Where(x => x.UserId == user.Id).Select(x => x.ProviderKey).FirstOrDefault();

            var identityResult = await userManager.RemoveLoginAsync(user, activeDirectoryOptions.LoginProvider, userGuid);
            identityResult.EnsureSuccess();

            if (identityResult.Succeeded) {
                response = true;
            }

            return response;
        }

        private async Task<bool> AddUserLogin(User user, string userADGuid) {
            bool response = false;
            var userLoginInfo = new UserLoginInfo(activeDirectoryOptions.LoginProvider, userADGuid, activeDirectoryOptions.ActiveDirectoryName);

            var identityResult = await userManager.AddLoginAsync(user, userLoginInfo);
            identityResult.EnsureSuccess();

            if (identityResult.Succeeded) {
                response = true;
            }

            return response;
        }

        private async Task<bool> AddUser(User user) {

            bool response = false;

            var identityResult = await userManager.CreateAsync(user);
            identityResult.EnsureSuccess();

            if (identityResult.Succeeded) {
                response = true;
            }

            return response;
        }
    }
}
