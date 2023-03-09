using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Identity.Login {
   
    public class LoginUserRequest : IRequest<IRequestResponse<LoginUserResponse>> {
    
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }

        //public string Domain { get; set; }

        //public string LogonName
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(Domain))
        //            return UserName;

        //        return $"{Domain}{UserName}";
        //    }
        //}
    }
}
