using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class SafetyStudyPlanPlaneFile {

        public SafetyStudyPlanPlaneFile() {
            
            Data_Id = Guid.NewGuid();
        }

        public int Id { get; set; }

        public Guid Data_Id { get; set; }

        public byte[] Data { get; set; }

        public int? Record_Id { get; set; }

        public string FileName { get; set; }

        public string Name { get; set; }



        public int SafetyStudyPlanPlaneId { get; set; }

        public SafetyStudyPlanPlane SafetyStudyPlanPlane { get; set; }
    }
}
