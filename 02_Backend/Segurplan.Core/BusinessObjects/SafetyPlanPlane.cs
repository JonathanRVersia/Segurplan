using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.Core.BusinessObjects {
    public class SafetyPlanPlane {

        public int Id { get; set; }

        public int IdPlan { get; set; }

        public int? IdPlane { get; set; }

        public string Description { get; set; }

        public string FamilyName { get; set; }

        public string FileName { get; set; }

        public int Position { get; set; }

        public byte[] Data { get; set; }

        public bool ContainsFile { get; set; }

        public bool IsAvailable { get; set; }
    }
}
