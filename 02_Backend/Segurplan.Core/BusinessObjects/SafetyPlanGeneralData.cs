
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Segurplan.Core.BusinessObjects {
    public class SafetyPlanGeneralData {

        //[Required]
        [DisplayName("DetallePlan.DatGen.Proyecto.Nombre")]
        public string PlanTitle { get; set; }

        [Required]
        public int? IdBusinessAddress { get; set; }

        public string BusinessAddress { get; set; }

        public string CenterName { get; set; }

        //[Required]
        public int? IdDelegation { get; set; }

        public string DelegationName { get; set; }

        public int? IdTemplate { get; set; }

        public string TemplateName { get; set; }

        public string Organization { get; set; }

        //[Required]
        public int? IdCustomer { get; set; }

        public string CustomerName { get; set; }

        //[Required]
        [DisplayName("DetallePlan.DatGen.Cliente.Nombre")]
        public string CustomerDescription { get; set; }

        //[Required]
        public int? IdGeneralActivity { get; set; }

        public string GeneralActivityName { get; set; }

        //[Required]
        public int? AffiliatedId { get; set; } = 1;

        //[Required]
        [DisplayName("DetallePlan.DatGen.UTE")]
        public string AffiliatedName { get; set; }

        //[Required]
        public int? IdReviewer { get; set; }

        public string ReviewerName { get; set; }

        public bool IsEvaluation { get; set; }

        public string ApproverName { get; set; }

        //[Required]
        [DisplayName("DetallePlan.DatGen.PersonaElaboracion")]
        public string CreatorName { get; set; }

        //[Required]
        public int? CreatorCategoryId { get; set; }

        public string CreatorCategoryName { get; set; }

        public string ModifiedByName { get; set; }

        //public PlanFile Anagram { get; set; }
        public List<PlanFile> Anagrams { get; set; } = new List<PlanFile>();

        public List<IFormFile> AnagramUploadFiles { get; set; }

        //public bool DeleteExistingFile { get; set; }
        public string DeleteExistingFileIdsCsv { get; set; }
        public List<int> DeleteExistingFileIds { get; set; }
    }
}
