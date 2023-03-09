using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.Core.BusinessObjects {
    public class ApplicationPlaneFamily {

        public int FamilyId { get; set; }

        public string FamilyName { get; set; }

        public List<ApplicationPlane> PlaneList { get; set; }
    }
}
