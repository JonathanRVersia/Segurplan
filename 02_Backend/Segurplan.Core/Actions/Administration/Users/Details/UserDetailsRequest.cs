using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Users.Details {
    public class UserDetailsRequest : IRequest<IRequestResponse<UserDetailsResponse>> {
        public int Id { get; set; }
    }
}
