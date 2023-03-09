using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class DefaultSafetyStudyPlanFile {

        public int Id { get; set; }

        public int IdPlanFileType { get; set; }

        public string FileName { get; set; }

        public byte[] FileData { get; set; }
        public decimal FileSize { get; set; }
    }
}
