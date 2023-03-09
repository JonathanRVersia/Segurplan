using MediatR;
using Segurplan.Core.Actions.Administration.Users.Details;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Users.UpdateUser {
    public class UpdateUserRequest : IRequest<IRequestResponse<UserDetailsResponse>> {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string CompleteName { get; set; }
        public string UserRole { get; set; }
        public string Email { get; set; }
        public string UserADGuid { get; set; }
        public bool IsSuscribed { get; set; }
    }
}
