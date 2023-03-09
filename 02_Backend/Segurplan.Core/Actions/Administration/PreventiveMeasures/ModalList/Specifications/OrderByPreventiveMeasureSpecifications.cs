
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Segurplan.FrameworkExtensions.Query;

namespace Segurplan.Core.Actions.Administration.PreventiveMeasures.ModalList.Specifications {
    public class OrderByPreventiveMeasureSpecifications : Specification<PreventiveMeasureListResponse.ListItem> {
        public void ByCode() => OrderBy(c =>int.Parse(c.Code));
        public void ByCodeDesc() => ApplyOrderByDescending(c =>int.Parse(c.Code));
    }
}
