using Segurplan.Core.Actions.Administration;

namespace Segurplan.Web.Pages.Components.UserDetails {
    public class UserDetailsViewComponentModel {
        public UserDetailsModel UserDetailsModel { get; set; } = new UserDetailsModel();
        public AdministrationActionType Action { get; set; }
        public string PageHandler { get; set; }
        //Used to print error on Creation when user is not on AD or already is in DB
        public string ErrorMsg { get; set; }

        public bool IsEditMode { get; set; }
    }
}
