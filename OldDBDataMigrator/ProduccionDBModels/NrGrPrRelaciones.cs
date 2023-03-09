using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class NrGrPrRelaciones
    {
        public int Id { get; set; }
        public int? IdNivel { get; set; }
        public int? IdProbabilidad { get; set; }
        public int? IdGravedad { get; set; }
        public DateTime? FecCreacion { get; set; }
        public DateTime? FecModificacion { get; set; }
        public int? IdUsuario { get; set; }

        public Gravedades IdGravedadNavigation { get; set; }
        public NivelesRiesgo IdNivelNavigation { get; set; }
        public ProbabilidadesRiesgo IdProbabilidadNavigation { get; set; }
        public Usuarios IdUsuarioNavigation { get; set; }
    }
}
