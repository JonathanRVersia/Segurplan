using Newtonsoft.Json;

namespace Segurplan.Core.Actions.Plans.PlansData {
    public class TabContenidos {
        public string PlanCreationCompany { get; set; } = "No Comp corporation S.A";

        public string Promoter { get; set; } = "Iberdrola";

        public string WorkLocation { get; set; } = "Paseo de la castellana 23";


        public int ImplementationPeriodDays { get; set; } = 240;

        public int ImplementationPeriodYears { get; set; } = 1;

        public decimal budgetPSS { get; set; } = 2312345.24M;

        public int MaxWorkers { get; set; } = 75;

        public decimal AVGWorkers { get; set; } = 30;

        public string Hierarchy { get; set; } = "SOME text wiht hierarchy";

        public string WorkDescAndLocation { get; set; }

        public string ActivityDesc { get; set; }

        public string InterferencesAndAfectedServices { get; set; }


        public string NearbyHealthcareCenter { get; set; }
        public string ImplementationPeriodAndworkforce { get; set; }

        public int EmergencyPlanSize { get; set; } = 1;

        public decimal TotalSecurityBudget { get; set; } = 4564563455476.45M;

        public TabContenidos() {

        }
        public override string ToString() {
            return JsonConvert.SerializeObject(this);
        }
    }
}
