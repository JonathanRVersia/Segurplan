using MediatR;
using Segurplan.Core.Actions.Actions.Administration.Users;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Users {
    public class UserListRequest : IRequest<IRequestResponse<UserListResponse>> {

        public AdministrationActionType UserOperation { get; set; }
        public UsersListTableState TableState { get; set; }
        public UsersFilter TableFilter { get; set; }
        public UserListRequest(UsersListTableState tableState, UsersFilter tableFilter) {

            TableState = tableState;
            TableFilter = tableFilter;
        }
    }
}
