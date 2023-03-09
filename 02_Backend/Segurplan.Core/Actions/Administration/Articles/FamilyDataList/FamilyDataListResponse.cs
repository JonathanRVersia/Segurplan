using System.Collections.Generic;
using Segurplan.Core.BusinessObjects;

namespace Segurplan.Core.Actions.Administration.Articles.FamilyDataList {
    public class FamilyDataListResponse {
        public List<ApplicationArticleFamily> Family { get; set; }
        public FamilyDataListResponse(List<ApplicationArticleFamily> family) {
            Family = family;
        }
    }
}
