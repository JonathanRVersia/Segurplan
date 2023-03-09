using System.Collections.Generic;

namespace Segurplan.Core.Actions.Plans.PlansData {
    public class PlanArticle {

        public int Id { get; set; }

        public string Name { get; set; }

        public int Percentage { get; set; }

        //Decimal
        public string TimeOfWork { get; set; }

        public int MinimumUnit { get; set; }

        //Decimal
        public string Price { get; set; }

        //Decimal
        public string AmortizationTime { get; set; }

        //Decimal
        public string AmortizationPrice { get; set; }

        //Decimal
        public string PriceDurationWork { get; set; }

        public int Unit { get; set; }

        //Decimal
        public string TotalPrice { get; set; }

        public int IdArticleFamily { get; set; }
    }
}
