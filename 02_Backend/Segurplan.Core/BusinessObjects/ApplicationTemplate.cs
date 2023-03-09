using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Segurplan.DataAccessLayer.Enums;

namespace Segurplan.Core.BusinessObjects {
    public class ApplicationTemplate {
        public ApplicationTemplate() {}

        public ApplicationTemplate(int id, string name, string notes, string filePath, int createdBy, string creationDate, int modifiedBy, string modifiedDate, IFormFile archivo, byte[] fileData) {
            Id = id;
            Name = name;
            Notes = notes;
            FilePath = filePath;
            CreatedBy = createdBy;
            CreationDate = creationDate;
            ModifiedBy = modifiedBy;
            ModifiedDate = modifiedDate;
            Archivo = archivo;
            FileData = fileData;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Notes { get; set; }

        public string FilePath { get; set; }

        public int CreatedBy { get; set; }

        public string CreatorName { get; set; }

        public string CreationDate { get; set; }

        public string ModifiedDate { get; set; }

        public int ModifiedBy { get; set; }

        public string ModifiedByName { get; set; }

        public PlanFile FileDetails { get; set; }

        public IFormFile Archivo { get; set; }

        public byte[] FileData { get; set; }

        public decimal FileSize { get; set; }

        public bool DeleteExistingFile { get; set; }

        public TemplateType TemplateType { get; set; }
    }
}
