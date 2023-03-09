using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Users.Details {
    public class UserDetailsRequestHandler : IRequestHandler<UserDetailsRequest, IRequestResponse<UserDetailsResponse>> {

        private readonly UserManager<User> userManager;

        public UserDetailsRequestHandler(UserManager<User> userManager) {
            this.userManager = userManager;
        }

        public async Task<IRequestResponse<UserDetailsResponse>> Handle(UserDetailsRequest request, CancellationToken cancellationToken) {

            var user = await userManager.FindByIdAsync(request.Id.ToString());
            var userLogin = await userManager.GetLoginsAsync(user);
            var roles = await userManager.GetRolesAsync(user);

            return RequestResponse.Ok(new UserDetailsResponse {
                Id = user.Id,
                CompleteName = user.CompleteName,
                CreateDate = user.CreateDate,
                ModifiedDate = user.ModifiedDate,
                Email = user.Email,
                IsSuscribed = userLogin.Any(),
                UserName = user.UserName,
                UserRole = roles.FirstOrDefault()
            });
        }
    }
}
