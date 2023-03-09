using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class Actividades
    {
        public Actividades()
        {
            AsignacionesEpi = new HashSet<AsignacionesEpi>();
            Evaluaciones = new HashSet<Evaluaciones>();
            EvaluacionesMedida = new HashSet<EvaluacionesMedida>();
        }

        public int Id { get; set; }
        public string Actividad { get; set; }
        public int? IdCapitulo { get; set; }
        public int? IdSubcapitulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FecCreacion { get; set; }
        public DateTime? FecModificacion { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdEstado { get; set; }
        public string DescripcionWord { get; set; }
        public int? IdActividadAnt { get; set; }
        public string OrganizacionTrabajo { get; set; }
        public string EvaluacionRiesgos { get; set; }
        public string MaquinaHerramienta { get; set; }
        public string DescripcionTrabajo { get; set; }
        public string DetallesAsociados { get; set; }
        public string MediosAuxiliares { get; set; }

        public Capitulos IdCapituloNavigation { get; set; }
        public Subcapitulos IdSubcapituloNavigation { get; set; }
        public Usuarios IdUsuarioNavigation { get; set; }
        public ICollection<AsignacionesEpi> AsignacionesEpi { get; set; }
        public ICollection<Evaluaciones> Evaluaciones { get; set; }
        public ICollection<EvaluacionesMedida> EvaluacionesMedida { get; set; }
    }
}
