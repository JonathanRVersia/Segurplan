using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Segurplan.Web.Pages.Models.Account {
    public class LoginData {

        [Required]
        [DataType(DataType.Text)]
        [DisplayName("Identity.Login.UserName")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Identity.Login.Password")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
