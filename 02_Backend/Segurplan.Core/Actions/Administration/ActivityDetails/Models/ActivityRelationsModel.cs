using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.Core.Actions.Administration.ActivityDetails.Models {
    public class ActivityRelationsModel {
        public int Id { get; set; }
        public int IdRelations { get; set; }
        public int IdChapterRelation { get; set; }
        public int IdSubChapterRelation { get; set; }
        public int IdActivityRelation { get; set; }
    }
}
