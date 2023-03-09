using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Segurplan.Core.Helpers.ActiveDirectory;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Users.CreateUserFromAD {
    public class CreateUserFromADRequestHandler : IRequestHandler<CreateUserFromADRequest, IRequestResponse<CreateUserFromADResponse>> {

        private readonly UserManager<User> userManager;
        private readonly ActiveDirectoryService activeDirectoryService;

        public CreateUserFromADRequestHandler(UserManager<User> userManager, ActiveDirectoryService activeDirectoryService) {
            this.userManager = userManager;
            this.activeDirectoryService = activeDirectoryService;
        }

        public async Task<IRequestResponse<CreateUserFromADResponse>> Handle(CreateUserFromADRequest request, CancellationToken cancellationToken) {

            var response = new CreateUserFromADResponse();

            if (!await CheckIfUserExistsInDB(request.UserName)) {
                var adUser = GetADUser(request.UserName);

                if (adUser != null) {

                    response.UserName = adUser.UserName;
                    response.Email = adUser.UserEmail;
                    response.CompleteName = adUser.Name;
                    response.UserADGuid = adUser.UserGuid;

                } else
                    response.NotExistsInAd = true;

            } else
                response.ExistsInDB = true;

            return RequestResponse.Ok(response);
        }


        private async Task<bool> CheckIfUserExistsInDB(string userName) {

            return await userManager.FindByNameAsync(userName) != null;
        }

        private ActiveDirectoryInfo GetADUser(string userName) {

            return activeDirectoryService.GetActiveDirectoryUser(userName);
        }
    }
}
