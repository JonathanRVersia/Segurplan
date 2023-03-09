using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class Capitulos
    {
        public Capitulos()
        {
            Actividades = new HashSet<Actividades>();
            AsignacionesEpi = new HashSet<AsignacionesEpi>();
            Evaluaciones = new HashSet<Evaluaciones>();
            EvaluacionesMedida = new HashSet<EvaluacionesMedida>();
        }

        public int Id { get; set; }
        public int? Capitulo { get; set; }
        public string Titulo { get; set; }
        public int? NumRevision { get; set; }
        public DateTime? FecRevision { get; set; }
        public DateTime? FecRevisionAnt { get; set; }
        public int? IdElaborador1 { get; set; }
        public int? IdElaborador2 { get; set; }
        public int? IdComprobador { get; set; }
        public int? IdAprobador { get; set; }
        public DateTime? FecCreacion { get; set; }
        public DateTime? FecModificacion { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdEstado { get; set; }
        public int? IdCapituloAnt { get; set; }
        public string DescripcionWord { get; set; }
        public string DescripcionTrabajo { get; set; }
        public string OrganizacionTrabajo { get; set; }
        public string EvaluacionRiesgos { get; set; }
        public string MaquinaHerramienta { get; set; }
        public string DetallesAsociados { get; set; }
        public string MediosAuxiliares { get; set; }

        public Usuarios IdAprobadorNavigation { get; set; }
        public Usuarios IdComprobadorNavigation { get; set; }
        public Usuarios IdElaborador1Navigation { get; set; }
        public Usuarios IdElaborador2Navigation { get; set; }
        public Usuarios IdUsuarioNavigation { get; set; }
        public ICollection<Actividades> Actividades { get; set; }
        public ICollection<AsignacionesEpi> AsignacionesEpi { get; set; }
        public ICollection<Evaluaciones> Evaluaciones { get; set; }
        public ICollection<EvaluacionesMedida> EvaluacionesMedida { get; set; }
    }
}
