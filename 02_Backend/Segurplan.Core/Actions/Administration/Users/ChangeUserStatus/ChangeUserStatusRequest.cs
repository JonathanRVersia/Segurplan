using MediatR;
using Segurplan.Core.Helpers.ActiveDirectory;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Users.ChangeUserStatus {
    public class ChangeUserStatusRequest : IRequest<IRequestResponse<bool>> {
        public int UserId { get; set; }
        public bool IsRegister { get; set; }
        public User DbUser { get; set; }

    }
}
