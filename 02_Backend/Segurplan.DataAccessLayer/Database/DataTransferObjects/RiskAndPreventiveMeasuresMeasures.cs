namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class RiskAndPreventiveMeasuresMeasures {

        public int Id { get; set; }


        public int RisksAndPreventiveMeasuresId { get; set; }
        public RisksAndPreventiveMeasures RisksAndPreventiveMeasures { get; set; }


        public int PreventiveMeasureId { get; set; }
        public PreventiveMeasure PreventiveMeasure { get; set; }
        public int PreventiveMeasureOrder { get; set; }

    }
}
