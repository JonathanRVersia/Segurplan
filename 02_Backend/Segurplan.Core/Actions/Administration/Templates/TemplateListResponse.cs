using System.Collections.Generic;
using Segurplan.Core.BusinessObjects;

namespace Segurplan.Core.Actions.Administration.Templates {
    public class TemplateListResponse {

        public List<ApplicationTemplate> TemplateList { get; set; } = new List<ApplicationTemplate>();

        public int TotalRows { get; set; }

        public TemplateListResponse(List<ApplicationTemplate> templateList, int totalRows) {
            TemplateList = templateList;
            TotalRows = totalRows;
        }

        public class ListItem {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Notes { get; set; }
        }
    }
}
