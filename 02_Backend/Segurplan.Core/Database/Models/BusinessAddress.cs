using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Segurplan.Core.Domain.Identity;

namespace Segurplan.Core.Database.Models {
    public partial class BusinessAddress {
        public BusinessAddress() {
            Center = new HashSet<Center>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(10)]
        public string Code { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        public int CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdateDate { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("BusinessAddressCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("BusinessAddressModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }
        [InverseProperty("IdAddressNavigation")]
        public ICollection<Center> Center { get; set; }
    }
}
