using Segurplan.Core.Actions.Administration.Users.CreateUserFromAD;
using Segurplan.Core.Actions.Administration.Users.Details;
using Segurplan.Core.Actions.Administration.Users.UpdateUser;
using Segurplan.Web.Pages.Components.UserDetails;

namespace Segurplan.Web.Pages.Models.Administration.Users {
    public class UsersDetailsProfiles : AutoMapper.Profile {

        public UsersDetailsProfiles() {
            CreateMap<CreateUserFromADResponse, UserDetailsModel>();
            CreateMap<UserDetailsModel, UpdateUserRequest>();
            CreateMap<UserDetailsResponse, UserDetailsModel>();
        }
    }
}
