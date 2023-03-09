using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Segurplan.Core.Database;
using Segurplan.Core.Helpers.ActiveDirectory;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.FrameworkExtensions.Identity;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Users.ChangeUserStatus {
    public class ChangeUserStatusRequestHandler : IRequestHandler<ChangeUserStatusRequest, IRequestResponse<bool>> {

        private readonly UserManager<User> userManager;
        private readonly ActiveDirectoryService activeDirectoryService;
        private readonly ActiveDirectoryOptions activeDirectoryOptions;
        private readonly SegurplanContext context;

        public ChangeUserStatusRequestHandler(UserManager<User> userManager, ActiveDirectoryService activeDirectoryService, ActiveDirectoryOptions activeDirectoryOptions, SegurplanContext context) {
            this.userManager = userManager;
            this.activeDirectoryService = activeDirectoryService;
            this.activeDirectoryOptions = activeDirectoryOptions;
            this.context = context;
        }

        public async Task<IRequestResponse<bool>> Handle(ChangeUserStatusRequest request, CancellationToken cancellationToken) {

            if (request.DbUser == null)
                request.DbUser = await userManager.FindByIdAsync(request.UserId.ToString());

            var userInfo = activeDirectoryService.GetActiveDirectoryUser(request.DbUser.UserName);
            if (request.IsRegister) {
                if(userInfo!=null)
                    await Register(request.DbUser, activeDirectoryService.GetActiveDirectoryUser(request.DbUser.UserName));

            } else
                await UnSubscribe(request.DbUser);

            return RequestResponse.Ok(userInfo == null && request.IsRegister);
        }

        private async Task Register(User dbUser, ActiveDirectoryInfo userADInfo) {

            var userLoginInfo = new UserLoginInfo(activeDirectoryOptions.LoginProvider, userADInfo.UserGuid, activeDirectoryOptions.ActiveDirectoryName);
            var identityResult = await userManager.AddLoginAsync(dbUser, userLoginInfo);
            identityResult.EnsureSuccess();
        }

        private async Task UnSubscribe(User dbUser) {

            var userGuid = context.UserLogins.Where(x => x.UserId == dbUser.Id).Select(x => x.ProviderKey).FirstOrDefault();

            var identityResult = await userManager.RemoveLoginAsync(dbUser, activeDirectoryOptions.LoginProvider, userGuid);
            identityResult.EnsureSuccess();
        }
    }
}
