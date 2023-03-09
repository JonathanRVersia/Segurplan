using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Segurplan.Core.Actions.Administration;
using Segurplan.Core.Actions.Administration.Users;
using Segurplan.Core.Actions.Administration.Users.ChangeUserStatus;
using Segurplan.Core.Actions.Administration.Users.GetUserIdsFromLoginInfo;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.Web.Pages.Models.Administration.Users {
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]


    public class UserManagementList : PageModel {

        private readonly IMediator mediator;

        public UsersFilter TableFilter { get; set; } = null;
        public UsersListTableState TableState { get; set; }
        public List<ApplicationUser> UserList { get; set; }
        public AdministrationActionType CurrentOperation { get; set; }
        public int TotalRows { get; set; }
        public List<int> SuscribedUserIds { get; set; } = new List<int>();
        public bool ErrorMsg { get; set; }
        public UserManagementList(IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpGet("indexPage, tableRows, orderMode, orderBy, op, userName, name, userRol")]
        public async Task OnGetAsync(int indexPage = 0, int tableRows = 15, string orderMode = "asc", string orderBy = "name", AdministrationActionType op = AdministrationActionType.Read,
                                     string userId = "", string userName = "", string name = "", string userRol = "",bool ErrorMsgs = false) {
            if (tableRows < 15)
                tableRows = 15;
            //Check if any filter must be aplied
            if (!string.IsNullOrEmpty(userId) || !string.IsNullOrEmpty(userName) || !string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(userRol)) {
                TableFilter = new UsersFilter() { InUse = true, UserName = userName, Name = name, UserRol = userRol };
            } else {
                TableFilter = new UsersFilter() { InUse = false };
            }


            TableState = new UsersListTableState(indexPage, tableRows, orderMode, orderBy);
            CurrentOperation = op;
            ErrorMsg = ErrorMsgs;
            await UpdateTable().ConfigureAwait(true);
        }

        public async Task<IActionResult> UpdateTable() {

            var response = await mediator.Send(new UserListRequest(TableState, TableFilter)).ConfigureAwait(true);
            UserList = response.Value.UserList;
            TotalRows = response.Value.TotalRows;

            var userIdsResponse = await mediator.Send(new GetUserIdsFromLoginInfoRequest()).ConfigureAwait(true);

            if (userIdsResponse.Status == FrameworkExtensions.MediatR.RequestStatus.Ok)
                SuscribedUserIds = userIdsResponse.Value.UserIds;

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateUser(int? registerUserId, int? unsubscribeUserId) {

            int userId = registerUserId != null ? registerUserId ?? 0 : unsubscribeUserId ?? 0;
            bool isRegister = registerUserId != null ? true : false;

            var response = await mediator.Send(new ChangeUserStatusRequest { UserId = userId, IsRegister = isRegister }).ConfigureAwait(true);

            return new LocalRedirectResult($"/UserList?handler=OnGetAsync&ErrorMsgs={response.Value}");
        }
    }
}
