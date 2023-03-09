using Microsoft.AspNetCore.Mvc;
using Segurplan.Core.Actions.Administration;

namespace Segurplan.Web.Pages.Components.UserDetails {
    public class UserDetailsViewComponent : ViewComponent {

        public UserDetailsViewComponentModel UserDetails { get; set; } = new UserDetailsViewComponentModel();

        public IViewComponentResult Invoke(UserDetailsViewComponentModel userDetails) {

            if (userDetails != null)
                UserDetails = userDetails;

            switch (UserDetails.Action) {
                case AdministrationActionType.Create:
                    return CreateUser();
                case AdministrationActionType.Update:
                    return UpdateUser();
                case AdministrationActionType.Read:
                    return ViewUser();
                default:
                    return null;
            }
        }

        private IViewComponentResult CreateUser() {

            UserDetails.PageHandler = "CreateUser";

            return View("CreateUserView", UserDetails);
        }

        private IViewComponentResult UpdateUser() {

            UserDetails.PageHandler = "UpdateUser";
            UserDetails.IsEditMode = true;

            return View("UpdateUserView", UserDetails);
        }

        private IViewComponentResult ViewUser() {

            UserDetails.IsEditMode = false;

            return View("UpdateUserView", UserDetails);
        }
    }
}
