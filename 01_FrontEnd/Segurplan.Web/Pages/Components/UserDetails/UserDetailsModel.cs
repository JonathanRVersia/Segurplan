using System;
using System.ComponentModel.DataAnnotations;

namespace Segurplan.Web.Pages.Components.UserDetails {
    public class UserDetailsModel {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string CompleteName { get; set; }
        [Required]
        public string UserRole { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string UserADGuid { get; set; }
        public bool IsSuscribed { get; set; }
    }
}
