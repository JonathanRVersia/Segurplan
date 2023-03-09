using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class Usuarios
    {
        public Usuarios()
        {
            Actividades = new HashSet<Actividades>();
            Articulos = new HashSet<Articulos>();
            AsignacionesEpi = new HashSet<AsignacionesEpi>();
            CapitulosIdAprobadorNavigation = new HashSet<Capitulos>();
            CapitulosIdComprobadorNavigation = new HashSet<Capitulos>();
            CapitulosIdElaborador1Navigation = new HashSet<Capitulos>();
            CapitulosIdElaborador2Navigation = new HashSet<Capitulos>();
            CapitulosIdUsuarioNavigation = new HashSet<Capitulos>();
            Cargos = new HashSet<Cargos>();
            Centros = new HashSet<Centros>();
            ClasificacionesPlan = new HashSet<ClasificacionesPlan>();
            Delegaciones = new HashSet<Delegaciones>();
            Departamentos = new HashSet<Departamentos>();
            DireccionesNegocio = new HashSet<DireccionesNegocio>();
            Evaluaciones = new HashSet<Evaluaciones>();
            EvaluacionesMedida = new HashSet<EvaluacionesMedida>();
            Gravedades = new HashSet<Gravedades>();
            InverseIdUsuarioNavigation = new HashSet<Usuarios>();
            Medidas = new HashSet<Medidas>();
            Monedas = new HashSet<Monedas>();
            NivelesRiesgo = new HashSet<NivelesRiesgo>();
            NrGrPrRelaciones = new HashSet<NrGrPrRelaciones>();
            PlanesActividades = new HashSet<PlanesActividades>();
            PlanesIdRealizadorNavigation = new HashSet<Planes>();
            PlanesIdRevisadorNavigation = new HashSet<Planes>();
            PlanesIdUsuarioNavigation = new HashSet<Planes>();
            PlanesPlanos = new HashSet<PlanesPlanos>();
            PlanesPresupuestos = new HashSet<PlanesPresupuestos>();
            PlanosGenerales = new HashSet<PlanosGenerales>();
            Plantillas = new HashSet<Plantillas>();
            ProbabilidadesRiesgo = new HashSet<ProbabilidadesRiesgo>();
            Riesgos = new HashSet<Riesgos>();
            Subcapitulos = new HashSet<Subcapitulos>();
            TiposArticulo = new HashSet<TiposArticulo>();
        }

        public int Id { get; set; }
        public int? Codigo { get; set; }
        public string Nombre { get; set; }
        public string Usuario { get; set; }
        public string Clave { get; set; }
        public string EMail { get; set; }
        public int? Perfil { get; set; }
        public int? IdDepartamento { get; set; }
        public string Observaciones { get; set; }
        public string Tel { get; set; }
        public int? IdCargo { get; set; }
        public int? IdDireccion { get; set; }
        public bool? Administrador { get; set; }
        public DateTime? FecCreacion { get; set; }
        public DateTime? FecModificacion { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdTipoUsu { get; set; }

        public Cargos IdCargoNavigation { get; set; }
        public Departamentos IdDepartamentoNavigation { get; set; }
        public DireccionesNegocio IdDireccionNavigation { get; set; }
        public Usuarios IdUsuarioNavigation { get; set; }
        public ICollection<Actividades> Actividades { get; set; }
        public ICollection<Articulos> Articulos { get; set; }
        public ICollection<AsignacionesEpi> AsignacionesEpi { get; set; }
        public ICollection<Capitulos> CapitulosIdAprobadorNavigation { get; set; }
        public ICollection<Capitulos> CapitulosIdComprobadorNavigation { get; set; }
        public ICollection<Capitulos> CapitulosIdElaborador1Navigation { get; set; }
        public ICollection<Capitulos> CapitulosIdElaborador2Navigation { get; set; }
        public ICollection<Capitulos> CapitulosIdUsuarioNavigation { get; set; }
        public ICollection<Cargos> Cargos { get; set; }
        public ICollection<Centros> Centros { get; set; }
        public ICollection<ClasificacionesPlan> ClasificacionesPlan { get; set; }
        public ICollection<Delegaciones> Delegaciones { get; set; }
        public ICollection<Departamentos> Departamentos { get; set; }
        public ICollection<DireccionesNegocio> DireccionesNegocio { get; set; }
        public ICollection<Evaluaciones> Evaluaciones { get; set; }
        public ICollection<EvaluacionesMedida> EvaluacionesMedida { get; set; }
        public ICollection<Gravedades> Gravedades { get; set; }
        public ICollection<Usuarios> InverseIdUsuarioNavigation { get; set; }
        public ICollection<Medidas> Medidas { get; set; }
        public ICollection<Monedas> Monedas { get; set; }
        public ICollection<NivelesRiesgo> NivelesRiesgo { get; set; }
        public ICollection<NrGrPrRelaciones> NrGrPrRelaciones { get; set; }
        public ICollection<PlanesActividades> PlanesActividades { get; set; }
        public ICollection<Planes> PlanesIdRealizadorNavigation { get; set; }
        public ICollection<Planes> PlanesIdRevisadorNavigation { get; set; }
        public ICollection<Planes> PlanesIdUsuarioNavigation { get; set; }
        public ICollection<PlanesPlanos> PlanesPlanos { get; set; }
        public ICollection<PlanesPresupuestos> PlanesPresupuestos { get; set; }
        public ICollection<PlanosGenerales> PlanosGenerales { get; set; }
        public ICollection<Plantillas> Plantillas { get; set; }
        public ICollection<ProbabilidadesRiesgo> ProbabilidadesRiesgo { get; set; }
        public ICollection<Riesgos> Riesgos { get; set; }
        public ICollection<Subcapitulos> Subcapitulos { get; set; }
        public ICollection<TiposArticulo> TiposArticulo { get; set; }
    }
}
