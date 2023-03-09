using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class PlanesDatosParticulares
    {
        public int Id { get; set; }
        public string Txt1 { get; set; }
        public string Txt2 { get; set; }
        public double? Txt3 { get; set; }
        public double? Txt4 { get; set; }
        public int? Txt5 { get; set; }
        public string Txt6 { get; set; }
        public string Txt7 { get; set; }
        public double? Txt8 { get; set; }
        public double? Txt9 { get; set; }
        public double? Txt10 { get; set; }
        public string Txt11 { get; set; }
        public string Txt12 { get; set; }
        public double? Txt13 { get; set; }
        public string Txt14 { get; set; }
        public string Txt15 { get; set; }
        public bool? HayInterySer { get; set; }
        public int? IdPlan { get; set; }
        public int? TipoPlanEmergencia { get; set; }
        public string PresupuestoTotal { get; set; }
        public string PlazoEjecucionPrevision { get; set; }
        public string EstructuraOrganizativa { get; set; }

        public Planes IdPlanNavigation { get; set; }
    }
}
