using System.ComponentModel.DataAnnotations;

namespace Segurplan.Core.BusinessObjects {
    public class ApplicationUser {
        public int Id { get; set; }
        public string UserName { get; set; }
        //public string NormalizedName { get; set; }
        public string CompleteName { get; set; }
        public string UserRoleName { get; set; } = "N/A";
        public string CreateDate { get; set; }
        public string ModifiedDate { get; set; }
        public int UserRoleId { get; set; } = 99;
        public int CenterId { get; set; } = 1;
        public string CenterName { get; set; } = "Central";
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public bool IsNameOk { get; set; } = true;
        public bool IsEmailOk { get; set; } = true;
        public bool IsUserNameOk { get; set; } = true;
    }
}

