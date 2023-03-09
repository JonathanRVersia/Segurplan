using System.ComponentModel.DataAnnotations;

namespace Segurplan.Core.BusinessObjects {
    public class ApplicationPreventiveMeasure {
        public ApplicationPreventiveMeasure() {
        }

        public ApplicationPreventiveMeasure(int id, int code, string desciption, string creationDate, string modifiedDate, string completeName, int createdBy) {
            Id = id;
            Code = code;
            Desciption = desciption;
            CreationDate = creationDate;
            ModifiedDate = modifiedDate;
            CompleteName = completeName;
            CreatedBy = createdBy;
        }

        public int Id { get; set; }
        public int Code { get; set; }

        [DataType(DataType.MultilineText)]
        public string Desciption { get; set; }

        public string CreationDate { get; set; }

        public string ModifiedDate { get; set; }
        public int CreatedBy { get; set; }
        public string CompleteName { get; set; }//This is the name, not UserName

    }
}
