using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class Subcapitulos
    {
        public Subcapitulos()
        {
            Actividades = new HashSet<Actividades>();
            AsignacionesEpi = new HashSet<AsignacionesEpi>();
            Evaluaciones = new HashSet<Evaluaciones>();
            EvaluacionesMedida = new HashSet<EvaluacionesMedida>();
        }

        public int Id { get; set; }
        public int? IdCapitulo { get; set; }
        public int? SubCapitulo { get; set; }
        public string Titulo { get; set; }
        public bool? TieneTexto { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FecCreacion { get; set; }
        public DateTime? FecModificacion { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdEstado { get; set; }
        public int? IdSubcapituloAnt { get; set; }
        public string OrganizacionTrabajo { get; set; }
        public string EvaluacionRiesgos { get; set; }
        public string MaquinaHerramienta { get; set; }
        public string DescripcionTrabajo { get; set; }
        public string DetallesAsociados { get; set; }
        public string MediosAuxiliares { get; set; }

        public Usuarios IdUsuarioNavigation { get; set; }
        public ICollection<Actividades> Actividades { get; set; }
        public ICollection<AsignacionesEpi> AsignacionesEpi { get; set; }
        public ICollection<Evaluaciones> Evaluaciones { get; set; }
        public ICollection<EvaluacionesMedida> EvaluacionesMedida { get; set; }
    }
}
