using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class Planes
    {
        public Planes()
        {
            PlanesActividades = new HashSet<PlanesActividades>();
            PlanesDatosParticulares = new HashSet<PlanesDatosParticulares>();
            PlanesPresupuestos = new HashSet<PlanesPresupuestos>();
        }

        public int Id { get; set; }
        public int? IdCentro { get; set; }
        public string Proyecto { get; set; }
        public string Empresa { get; set; }
        public DateTime? Fecha { get; set; }
        public int? IdRealizador { get; set; }
        public int? IdRevisador { get; set; }
        public bool? EvaluacionMapa { get; set; }
        public int? IdNivelRiesgo { get; set; }
        public int? IdPlantilla { get; set; }
        public int? IdTipoPlan { get; set; }
        public DateTime? FecCreacion { get; set; }
        public DateTime? FecModificacion { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdClasificacion { get; set; }
        public int? IdDireccion { get; set; }
        public bool? ConDescripcionesWord { get; set; }
        public bool? EsPlan { get; set; }
        public int? Codigo { get; set; }
        public string Realizadopor { get; set; }
        public string Revisadopor { get; set; }
        public string Anagrama { get; set; }
        public bool? MostrarPlanRiesgo { get; set; }

        public Centros IdCentroNavigation { get; set; }
        public ClasificacionesPlan IdClasificacionNavigation { get; set; }
        public NivelesRiesgo IdNivelRiesgoNavigation { get; set; }
        public Plantillas IdPlantillaNavigation { get; set; }
        public Usuarios IdRealizadorNavigation { get; set; }
        public Usuarios IdRevisadorNavigation { get; set; }
        public Usuarios IdUsuarioNavigation { get; set; }
        public ICollection<PlanesActividades> PlanesActividades { get; set; }
        public ICollection<PlanesDatosParticulares> PlanesDatosParticulares { get; set; }
        public ICollection<PlanesPresupuestos> PlanesPresupuestos { get; set; }
    }
}
