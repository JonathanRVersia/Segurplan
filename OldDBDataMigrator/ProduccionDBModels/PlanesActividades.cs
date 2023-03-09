using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class PlanesActividades
    {
        public int Id { get; set; }
        public int? Capitulo { get; set; }
        public int? Subcapitulo { get; set; }
        public string Actividad { get; set; }
        public string Titulo { get; set; }
        public int? IdPlan { get; set; }
        public DateTime? FecCreacion { get; set; }
        public DateTime? FecModificacion { get; set; }
        public int? IdUsuario { get; set; }
        public int? Idaux { get; set; }
        public int? IdauxPadre { get; set; }
        public string Texto { get; set; }
        public string DescripcionWord { get; set; }
        public int? Posicion { get; set; }
        public int? IdCapitulo { get; set; }
        public int? IdSubcapitulo { get; set; }
        public int? IdActividad { get; set; }
        public int? IdEstado { get; set; }
        public string DescripcionTrabajo { get; set; }
        public string OrganizacionTrabajo { get; set; }
        public string MaquinaHerramienta { get; set; }
        public string EvaluacionRiesgos { get; set; }
        public string DetallesAsociados { get; set; }
        public string MediosAuxiliares { get; set; }

        public Planes IdPlanNavigation { get; set; }
        public Usuarios IdUsuarioNavigation { get; set; }
    }
}
