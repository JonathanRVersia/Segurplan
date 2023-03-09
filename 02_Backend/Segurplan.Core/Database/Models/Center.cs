using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Segurplan.Core.Domain.Identity;

namespace Segurplan.Core.Database.Models {
    public partial class Center {
        public Center() {
            SafetyStudyPlan = new HashSet<SafetyStudyPlan>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(10)]
        public string Code { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        public int IdDelegation { get; set; }
        public int IdAddress { get; set; }
        public int CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdateDate { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("CenterCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }
        [ForeignKey("IdAddress")]
        [InverseProperty("Center")]
        public BusinessAddress IdAddressNavigation { get; set; }
        [ForeignKey("IdDelegation")]
        [InverseProperty("Center")]
        public Delegation IdDelegationNavigation { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("CenterModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }
        [InverseProperty("IdCenterNavigation")]
        public ICollection<SafetyStudyPlan> SafetyStudyPlan { get; set; }
    }
}
