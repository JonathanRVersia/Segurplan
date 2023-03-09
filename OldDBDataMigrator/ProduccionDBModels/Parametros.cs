using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class Parametros
    {
        public Parametros()
        {
            ParametrosActividades = new HashSet<ParametrosActividades>();
        }

        public int Id { get; set; }
        public string RutaPlanos { get; set; }
        public string RutaPlantillas { get; set; }
        public string RutaAnagrama { get; set; }
        public string Anagrama { get; set; }
        public string TextoNointerySer { get; set; }
        public string TextoSiinterySer { get; set; }
        public string TextoPlanEmergenciaC { get; set; }
        public string TextoPlanEmergenciaM { get; set; }
        public string TextoPlanEmergenciaL { get; set; }
        public double? TamPlano { get; set; }
        public string TextoUsuarioSinPermiso { get; set; }
        public double? PresupuestoPssmax { get; set; }
        public int? NumTrabajadoresMax { get; set; }
        public double? PlazoEjecucionMax { get; set; }
        public string RutaDicEs { get; set; }
        public string RutaGraEs { get; set; }
        public string DescripcionObra { get; set; }
        public string PresupuestoTotal { get; set; }
        public string PlazoEjecucionPrevision { get; set; }
        public bool? MostrarProcesoConversion { get; set; }

        public ICollection<ParametrosActividades> ParametrosActividades { get; set; }
    }
}
