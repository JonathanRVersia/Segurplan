using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Files.View {

    public class ViewPlanPlaneFileResponse {

        public List<ViewPlanPlaneItem> files;

        public ViewPlanPlaneFileResponse(List<ViewPlanPlaneItem> result) {
            files = result;
        }

    }

    public class ViewPlanPlaneItem {

        public string name;

        public string fileName { get; set; }

        public byte[] data;

    }
}
