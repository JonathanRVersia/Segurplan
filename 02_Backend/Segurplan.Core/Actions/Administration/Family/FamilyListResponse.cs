using System;
using System.Collections.Generic;
using System.Text;
using Segurplan.Core.BusinessObjects;

namespace Segurplan.Core.Actions.Administration.Family {
    public class FamilyListResponse {
        public List<ApplicationFamily> FamilyList { get; set; }
        public int TotalRows { get; set; }

        public FamilyListResponse(List<ApplicationFamily> familyList, int totalRows) {
            this.FamilyList = familyList;
            this.TotalRows = totalRows;
        }
    }
}
