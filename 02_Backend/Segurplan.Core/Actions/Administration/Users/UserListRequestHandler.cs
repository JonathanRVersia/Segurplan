using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Segurplan.Core.Actions.Administration.Users;
using Segurplan.Core.BusinessManagers;
using Segurplan.DataAccessLayer.DataAccessManagers;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Actions.Administration.Users {
    public class UserListRequestHandler : IRequestHandler<UserListRequest, IRequestResponse<UserListResponse>> {


        private ApplicationUserListManager manager;
        private readonly UserRolesDam usrDam;
        public UserListRequestHandler(UserRolesDam usrDam) {
            this.usrDam = usrDam;
        }


        public async Task<IRequestResponse<UserListResponse>> Handle(UserListRequest request, CancellationToken cancellationToken) {


            return await ReadUsersInformation(request);

        }



        private async Task<IRequestResponse<UserListResponse>> ReadUsersInformation(UserListRequest request) {
            manager = new ApplicationUserListManager(usrDam);
            try {

                var userList = await manager.ApplicationUserList(request.TableState, request.TableFilter);

                return RequestResponse.Ok(new UserListResponse(userList, manager.TotalUsers));

            } catch (Exception e) {
                Debug.WriteLine(e.ToString());
                return RequestResponse.NotOk(new UserListResponse(null, -1));
            }

        }



    }
}
