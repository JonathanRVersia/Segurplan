using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Users.CreateUserFromAD {
    public class CreateUserFromADRequest : IRequest<IRequestResponse<CreateUserFromADResponse>> {

        public string UserName { get; set; }

    }
}
