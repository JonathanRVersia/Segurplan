using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class SegurplanProduccionContext : DbContext
    {
        public SegurplanProduccionContext()
        {
        }

        public SegurplanProduccionContext(DbContextOptions<SegurplanProduccionContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Actividades> Actividades { get; set; }
        public virtual DbSet<Articulos> Articulos { get; set; }
        public virtual DbSet<AsignacionesEpi> AsignacionesEpi { get; set; }
        public virtual DbSet<Ayuda> Ayuda { get; set; }
        public virtual DbSet<Capitulos> Capitulos { get; set; }
        public virtual DbSet<Cargos> Cargos { get; set; }
        public virtual DbSet<Centros> Centros { get; set; }
        public virtual DbSet<ClasificacionesPlan> ClasificacionesPlan { get; set; }
        public virtual DbSet<Delegaciones> Delegaciones { get; set; }
        public virtual DbSet<Departamentos> Departamentos { get; set; }
        public virtual DbSet<DireccionesNegocio> DireccionesNegocio { get; set; }
        public virtual DbSet<Evaluaciones> Evaluaciones { get; set; }
        public virtual DbSet<EvaluacionesEvalMedida> EvaluacionesEvalMedida { get; set; }
        public virtual DbSet<EvaluacionesMedida> EvaluacionesMedida { get; set; }
        public virtual DbSet<Familias> Familias { get; set; }
        public virtual DbSet<Gravedades> Gravedades { get; set; }
        public virtual DbSet<Medidas> Medidas { get; set; }
        public virtual DbSet<MedidasActividades> MedidasActividades { get; set; }
        public virtual DbSet<Monedas> Monedas { get; set; }
        public virtual DbSet<NivelesRiesgo> NivelesRiesgo { get; set; }
        public virtual DbSet<NrGrPrRelaciones> NrGrPrRelaciones { get; set; }
        public virtual DbSet<Parametros> Parametros { get; set; }
        public virtual DbSet<ParametrosActividades> ParametrosActividades { get; set; }
        public virtual DbSet<Planes> Planes { get; set; }
        public virtual DbSet<PlanesActividades> PlanesActividades { get; set; }
        public virtual DbSet<PlanesDatosParticulares> PlanesDatosParticulares { get; set; }
        public virtual DbSet<PlanesPlanos> PlanesPlanos { get; set; }
        public virtual DbSet<PlanesPresupuestos> PlanesPresupuestos { get; set; }
        public virtual DbSet<PlanosGenerales> PlanosGenerales { get; set; }
        public virtual DbSet<Plantillas> Plantillas { get; set; }
        public virtual DbSet<ProbabilidadesRiesgo> ProbabilidadesRiesgo { get; set; }
        public virtual DbSet<Riesgos> Riesgos { get; set; }
        public virtual DbSet<Subcapitulos> Subcapitulos { get; set; }
        public virtual DbSet<TiposArticulo> TiposArticulo { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }

        // Unable to generate entity type for table 'dbo.PlanosGeneralesAntiguo'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.EPIS'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Variables'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.AsignacionesMediosAuxiliares'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.MediosAuxiliares'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.UsuariosPermisos'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.vgMenus'. Please see the warning messages.

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=localhost;Initial Catalog=SegurplanProduccion;Integrated Security=True");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actividades>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Actividad)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.DescripcionTrabajo).IsUnicode(false);

                entity.Property(e => e.DescripcionWord).IsUnicode(false);

                entity.Property(e => e.DetallesAsociados).IsUnicode(false);

                entity.Property(e => e.EvaluacionRiesgos).IsUnicode(false);

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdActividadAnt).HasColumnName("idActividadAnt");

                entity.Property(e => e.IdCapitulo).HasColumnName("idCapitulo");

                entity.Property(e => e.IdEstado).HasColumnName("idEstado");

                entity.Property(e => e.IdSubcapitulo).HasColumnName("idSubcapitulo");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.MaquinaHerramienta).IsUnicode(false);

                entity.Property(e => e.MediosAuxiliares).IsUnicode(false);

                entity.Property(e => e.OrganizacionTrabajo).IsUnicode(false);

                entity.HasOne(d => d.IdCapituloNavigation)
                    .WithMany(p => p.Actividades)
                    .HasForeignKey(d => d.IdCapitulo)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Actividades_Capitulos");

                entity.HasOne(d => d.IdSubcapituloNavigation)
                    .WithMany(p => p.Actividades)
                    .HasForeignKey(d => d.IdSubcapitulo)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Actividades_Subcapitulos");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Actividades)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_Actividades_Usuarios");
            });

            modelBuilder.Entity<Articulos>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Articulo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdMoneda).HasColumnName("idMoneda");

                entity.Property(e => e.IdTipoArticulo).HasColumnName("idTipoArticulo");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.HasOne(d => d.IdMonedaNavigation)
                    .WithMany(p => p.Articulos)
                    .HasForeignKey(d => d.IdMoneda)
                    .HasConstraintName("FK_Articulos_Monedas");

                entity.HasOne(d => d.IdTipoArticuloNavigation)
                    .WithMany(p => p.Articulos)
                    .HasForeignKey(d => d.IdTipoArticulo)
                    .HasConstraintName("FK_Articulos_TiposArticulo");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Articulos)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_Articulos_Usuarios1");
            });

            modelBuilder.Entity<AsignacionesEpi>(entity =>
            {
                entity.ToTable("AsignacionesEPI");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DescripcionWord).IsUnicode(false);

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdActividad).HasColumnName("idActividad");

                entity.Property(e => e.IdCapitulo).HasColumnName("idCapitulo");

                entity.Property(e => e.IdEpi).HasColumnName("idEPI");

                entity.Property(e => e.IdEstado).HasColumnName("idEstado");

                entity.Property(e => e.IdNuevo).HasColumnName("idNuevo");

                entity.Property(e => e.IdSubcapitulo).HasColumnName("idSubcapitulo");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.OrdenEpi).HasColumnName("OrdenEPI");

                entity.HasOne(d => d.IdActividadNavigation)
                    .WithMany(p => p.AsignacionesEpi)
                    .HasForeignKey(d => d.IdActividad)
                    .HasConstraintName("FK_AsignacionesEPI_Actividades");

                entity.HasOne(d => d.IdCapituloNavigation)
                    .WithMany(p => p.AsignacionesEpi)
                    .HasForeignKey(d => d.IdCapitulo)
                    .HasConstraintName("FK_AsignacionesEPI_Capitulos");

                entity.HasOne(d => d.IdSubcapituloNavigation)
                    .WithMany(p => p.AsignacionesEpi)
                    .HasForeignKey(d => d.IdSubcapitulo)
                    .HasConstraintName("FK_AsignacionesEPI_Subcapitulos");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.AsignacionesEpi)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_AsignacionesEPI_Usuarios");
            });

            modelBuilder.Entity<Ayuda>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Contenido).IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Capitulos>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Capitulo).HasColumnName("capitulo");

                entity.Property(e => e.DescripcionTrabajo).IsUnicode(false);

                entity.Property(e => e.DescripcionWord).IsUnicode(false);

                entity.Property(e => e.DetallesAsociados).IsUnicode(false);

                entity.Property(e => e.EvaluacionRiesgos).IsUnicode(false);

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecRevision)
                    .HasColumnName("fecRevision")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecRevisionAnt)
                    .HasColumnName("fecRevisionAnt")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdAprobador).HasColumnName("idAprobador");

                entity.Property(e => e.IdCapituloAnt).HasColumnName("idCapituloAnt");

                entity.Property(e => e.IdComprobador).HasColumnName("idComprobador");

                entity.Property(e => e.IdElaborador1).HasColumnName("idElaborador1");

                entity.Property(e => e.IdElaborador2).HasColumnName("idElaborador2");

                entity.Property(e => e.IdEstado).HasColumnName("idEstado");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.MaquinaHerramienta).IsUnicode(false);

                entity.Property(e => e.MediosAuxiliares).IsUnicode(false);

                entity.Property(e => e.NumRevision).HasColumnName("numRevision");

                entity.Property(e => e.OrganizacionTrabajo).IsUnicode(false);

                entity.Property(e => e.Titulo)
                    .HasColumnName("titulo")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdAprobadorNavigation)
                    .WithMany(p => p.CapitulosIdAprobadorNavigation)
                    .HasForeignKey(d => d.IdAprobador)
                    .HasConstraintName("FK_Capitulos_Usuarios4");

                entity.HasOne(d => d.IdComprobadorNavigation)
                    .WithMany(p => p.CapitulosIdComprobadorNavigation)
                    .HasForeignKey(d => d.IdComprobador)
                    .HasConstraintName("FK_Capitulos_Usuarios1");

                entity.HasOne(d => d.IdElaborador1Navigation)
                    .WithMany(p => p.CapitulosIdElaborador1Navigation)
                    .HasForeignKey(d => d.IdElaborador1)
                    .HasConstraintName("FK_Capitulos_Usuarios3");

                entity.HasOne(d => d.IdElaborador2Navigation)
                    .WithMany(p => p.CapitulosIdElaborador2Navigation)
                    .HasForeignKey(d => d.IdElaborador2)
                    .HasConstraintName("FK_Capitulos_Usuarios2");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.CapitulosIdUsuarioNavigation)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_Capitulos_Usuarios");
            });

            modelBuilder.Entity<Cargos>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cargo)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Cargos)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_Cargos_Usuarios");
            });

            modelBuilder.Entity<Centros>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Centro)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdDelegacion).HasColumnName("idDelegacion");

                entity.Property(e => e.IdDireccion).HasColumnName("idDireccion");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.HasOne(d => d.IdDelegacionNavigation)
                    .WithMany(p => p.Centros)
                    .HasForeignKey(d => d.IdDelegacion)
                    .HasConstraintName("FK_Centros_Delegaciones");

                entity.HasOne(d => d.IdDireccionNavigation)
                    .WithMany(p => p.Centros)
                    .HasForeignKey(d => d.IdDireccion)
                    .HasConstraintName("FK_Centros_DireccionesNegocio");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Centros)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_Centros_Usuarios");
            });

            modelBuilder.Entity<ClasificacionesPlan>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Clasificacion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.ClasificacionesPlan)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_ClasificacionesPlan_Usuarios");
            });

            modelBuilder.Entity<Delegaciones>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Delegacion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Delegaciones)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_Delegaciones_Usuarios");
            });

            modelBuilder.Entity<Departamentos>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Departamento)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Departamentos)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_Departamentos_Usuarios");
            });

            modelBuilder.Entity<DireccionesNegocio>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.DireccionesNegocio)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_DireccionesNegocio_Usuarios");
            });

            modelBuilder.Entity<Evaluaciones>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DescripcionWord).IsUnicode(false);

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdActividad).HasColumnName("idActividad");

                entity.Property(e => e.IdCapitulo).HasColumnName("idCapitulo");

                entity.Property(e => e.IdEstado).HasColumnName("idEstado");

                entity.Property(e => e.IdGravedad).HasColumnName("idGravedad");

                entity.Property(e => e.IdNivelRiesgo).HasColumnName("idNivelRiesgo");

                entity.Property(e => e.IdNuevo).HasColumnName("idNuevo");

                entity.Property(e => e.IdProbabilidad).HasColumnName("idProbabilidad");

                entity.Property(e => e.IdRiesgo).HasColumnName("idRiesgo");

                entity.Property(e => e.IdSubcapitulo).HasColumnName("idSubcapitulo");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.HasOne(d => d.IdActividadNavigation)
                    .WithMany(p => p.Evaluaciones)
                    .HasForeignKey(d => d.IdActividad)
                    .HasConstraintName("FK_Evaluaciones_Actividades");

                entity.HasOne(d => d.IdCapituloNavigation)
                    .WithMany(p => p.Evaluaciones)
                    .HasForeignKey(d => d.IdCapitulo)
                    .HasConstraintName("FK_Evaluaciones_Capitulos");

                entity.HasOne(d => d.IdGravedadNavigation)
                    .WithMany(p => p.Evaluaciones)
                    .HasForeignKey(d => d.IdGravedad)
                    .HasConstraintName("FK_Evaluaciones_Gravedades");

                entity.HasOne(d => d.IdNivelRiesgoNavigation)
                    .WithMany(p => p.Evaluaciones)
                    .HasForeignKey(d => d.IdNivelRiesgo)
                    .HasConstraintName("FK_Evaluaciones_NivelesRiesgo");

                entity.HasOne(d => d.IdProbabilidadNavigation)
                    .WithMany(p => p.Evaluaciones)
                    .HasForeignKey(d => d.IdProbabilidad)
                    .HasConstraintName("FK_Evaluaciones_ProbabilidadesRiesgo");

                entity.HasOne(d => d.IdRiesgoNavigation)
                    .WithMany(p => p.Evaluaciones)
                    .HasForeignKey(d => d.IdRiesgo)
                    .HasConstraintName("FK_Evaluaciones_Riesgos");

                entity.HasOne(d => d.IdSubcapituloNavigation)
                    .WithMany(p => p.Evaluaciones)
                    .HasForeignKey(d => d.IdSubcapitulo)
                    .HasConstraintName("FK_Evaluaciones_Subcapitulos");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Evaluaciones)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_Evaluaciones_Usuarios");
            });

            modelBuilder.Entity<EvaluacionesEvalMedida>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdEvaluacion).HasColumnName("idEvaluacion");

                entity.Property(e => e.IdEvaluacionMedida).HasColumnName("idEvaluacionMedida");

                entity.Property(e => e.ModoImportar)
                    .HasColumnName("modoImportar")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdEvaluacionNavigation)
                    .WithMany(p => p.EvaluacionesEvalMedida)
                    .HasForeignKey(d => d.IdEvaluacion)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_EvaluacionesEvalMedida_EvaluacionesEvalMedida");
            });

            modelBuilder.Entity<EvaluacionesMedida>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Csa)
                    .HasColumnName("CSA")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificación)
                    .HasColumnName("fecModificación")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdActImp).HasColumnName("idActImp");

                entity.Property(e => e.IdActividad).HasColumnName("idActividad");

                entity.Property(e => e.IdCapImp).HasColumnName("idCapImp");

                entity.Property(e => e.IdCapitulo).HasColumnName("idCapitulo");

                entity.Property(e => e.IdEstado).HasColumnName("idEstado");

                entity.Property(e => e.IdEvaluacion).HasColumnName("idEvaluacion");

                entity.Property(e => e.IdMedida).HasColumnName("idMedida");

                entity.Property(e => e.IdNuevo).HasColumnName("idNuevo");

                entity.Property(e => e.IdRiesgo).HasColumnName("idRiesgo");

                entity.Property(e => e.IdSubImp).HasColumnName("idSubImp");

                entity.Property(e => e.IdSubcapitulo).HasColumnName("idSubcapitulo");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Idmodificacion).HasColumnName("idmodificacion");

                entity.HasOne(d => d.IdActividadNavigation)
                    .WithMany(p => p.EvaluacionesMedida)
                    .HasForeignKey(d => d.IdActividad)
                    .HasConstraintName("FK_EvaluacionesMedida_Actividades");

                entity.HasOne(d => d.IdCapituloNavigation)
                    .WithMany(p => p.EvaluacionesMedida)
                    .HasForeignKey(d => d.IdCapitulo)
                    .HasConstraintName("FK_EvaluacionesMedida_Capitulos");

                entity.HasOne(d => d.IdEvaluacionNavigation)
                    .WithMany(p => p.EvaluacionesMedida)
                    .HasForeignKey(d => d.IdEvaluacion)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_EvaluacionesMedida_Evaluaciones");

                entity.HasOne(d => d.IdMedidaNavigation)
                    .WithMany(p => p.EvaluacionesMedida)
                    .HasForeignKey(d => d.IdMedida)
                    .HasConstraintName("FK_EvaluacionesMedida_Medidas");

                entity.HasOne(d => d.IdRiesgoNavigation)
                    .WithMany(p => p.EvaluacionesMedida)
                    .HasForeignKey(d => d.IdRiesgo)
                    .HasConstraintName("FK_EvaluacionesMedida_Riesgos");

                entity.HasOne(d => d.IdSubcapituloNavigation)
                    .WithMany(p => p.EvaluacionesMedida)
                    .HasForeignKey(d => d.IdSubcapitulo)
                    .HasConstraintName("FK_EvaluacionesMedida_Subcapitulos");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.EvaluacionesMedida)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_EvaluacionesMedida_Usuarios");
            });

            modelBuilder.Entity<Familias>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Familia)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            });

            modelBuilder.Entity<Gravedades>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Codigo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.Gravedad)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Orden).HasColumnName("orden");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Gravedades)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_Gravedades_Usuarios");
            });

            modelBuilder.Entity<Medidas>(entity =>
            {
                entity.HasIndex(e => e.IdEstado)
                    .HasName("IX_Estados");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdEstado).HasColumnName("idEstado");

                entity.Property(e => e.IdNuevo).HasColumnName("idNuevo");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Medida).IsUnicode(false);

                entity.Property(e => e.MedidaSoloTexto).IsUnicode(false);

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Medidas)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_Medidas_Usuarios");
            });

            modelBuilder.Entity<MedidasActividades>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Actividad)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DescripcionWord).IsUnicode(false);

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdMedida).HasColumnName("idMedida");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Idaux).HasColumnName("idaux");

                entity.Property(e => e.IdauxPadre).HasColumnName("idauxPadre");

                entity.Property(e => e.Texto)
                    .HasMaxLength(2500)
                    .IsUnicode(false);

                entity.Property(e => e.Titulo)
                    .HasMaxLength(2500)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdMedidaNavigation)
                    .WithMany(p => p.MedidasActividades)
                    .HasForeignKey(d => d.IdMedida)
                    .HasConstraintName("FK_MedidasActividades_Medidas");
            });

            modelBuilder.Entity<Monedas>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Moneda)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Monedas)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_Monedas_Usuarios");
            });

            modelBuilder.Entity<NivelesRiesgo>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Codigo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Nivel)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Orden).HasColumnName("orden");

                entity.Property(e => e.Prioridad).HasColumnName("prioridad");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.NivelesRiesgo)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_NivelesRiesgo_Usuarios");
            });

            modelBuilder.Entity<NrGrPrRelaciones>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdGravedad).HasColumnName("idGravedad");

                entity.Property(e => e.IdNivel).HasColumnName("idNivel");

                entity.Property(e => e.IdProbabilidad).HasColumnName("idProbabilidad");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.HasOne(d => d.IdGravedadNavigation)
                    .WithMany(p => p.NrGrPrRelaciones)
                    .HasForeignKey(d => d.IdGravedad)
                    .HasConstraintName("FK_NrGrPrRelaciones_Gravedades");

                entity.HasOne(d => d.IdNivelNavigation)
                    .WithMany(p => p.NrGrPrRelaciones)
                    .HasForeignKey(d => d.IdNivel)
                    .HasConstraintName("FK_NrGrPrRelaciones_NivelesRiesgo");

                entity.HasOne(d => d.IdProbabilidadNavigation)
                    .WithMany(p => p.NrGrPrRelaciones)
                    .HasForeignKey(d => d.IdProbabilidad)
                    .HasConstraintName("FK_NrGrPrRelaciones_ProbabilidadesRiesgo");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.NrGrPrRelaciones)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_NrGrPrRelaciones_Usuarios");
            });

            modelBuilder.Entity<Parametros>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Anagrama)
                    .HasColumnName("anagrama")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DescripcionObra).IsUnicode(false);

                entity.Property(e => e.NumTrabajadoresMax).HasColumnName("numTrabajadoresMax");

                entity.Property(e => e.PlazoEjecucionMax).HasColumnName("plazoEjecucionMax");

                entity.Property(e => e.PlazoEjecucionPrevision).IsUnicode(false);

                entity.Property(e => e.PresupuestoPssmax).HasColumnName("presupuestoPSSMax");

                entity.Property(e => e.PresupuestoTotal).IsUnicode(false);

                entity.Property(e => e.RutaAnagrama)
                    .HasColumnName("rutaAnagrama")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.RutaDicEs)
                    .HasColumnName("rutaDicES")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.RutaGraEs)
                    .HasColumnName("rutaGraES")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.RutaPlanos)
                    .HasColumnName("rutaPlanos")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.RutaPlantillas)
                    .HasColumnName("rutaPlantillas")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.TamPlano).HasColumnName("tamPlano");

                entity.Property(e => e.TextoNointerySer)
                    .HasColumnName("textoNOInterySer")
                    .IsUnicode(false);

                entity.Property(e => e.TextoPlanEmergenciaC)
                    .HasColumnName("textoPlanEmergenciaC")
                    .IsUnicode(false);

                entity.Property(e => e.TextoPlanEmergenciaL)
                    .HasColumnName("textoPlanEmergenciaL")
                    .IsUnicode(false);

                entity.Property(e => e.TextoPlanEmergenciaM)
                    .HasColumnName("textoPlanEmergenciaM")
                    .IsUnicode(false);

                entity.Property(e => e.TextoSiinterySer)
                    .HasColumnName("textoSIInterySer")
                    .IsUnicode(false);

                entity.Property(e => e.TextoUsuarioSinPermiso)
                    .HasColumnName("textoUsuarioSinPermiso")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ParametrosActividades>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Actividad)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DescripcionWord).IsUnicode(false);

                entity.Property(e => e.IdActividad).HasColumnName("idActividad");

                entity.Property(e => e.IdCapitulo).HasColumnName("idCapitulo");

                entity.Property(e => e.IdEstado).HasColumnName("idEstado");

                entity.Property(e => e.IdParametro).HasColumnName("idParametro");

                entity.Property(e => e.IdSubcapitulo).HasColumnName("idSubcapitulo");

                entity.Property(e => e.Idaux).HasColumnName("idaux");

                entity.Property(e => e.IdauxPadre).HasColumnName("idauxPadre");

                entity.Property(e => e.Texto)
                    .HasMaxLength(2500)
                    .IsUnicode(false);

                entity.Property(e => e.Titulo)
                    .HasMaxLength(2500)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdParametroNavigation)
                    .WithMany(p => p.ParametrosActividades)
                    .HasForeignKey(d => d.IdParametro)
                    .HasConstraintName("FK_ParametrosActividades_ParametrosActividades");
            });

            modelBuilder.Entity<Planes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Anagrama)
                    .HasColumnName("anagrama")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ConDescripcionesWord).HasColumnName("conDescripcionesWord");

                entity.Property(e => e.Empresa)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EsPlan).HasColumnName("esPlan");

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdCentro).HasColumnName("idCentro");

                entity.Property(e => e.IdClasificacion).HasColumnName("idClasificacion");

                entity.Property(e => e.IdDireccion).HasColumnName("idDireccion");

                entity.Property(e => e.IdNivelRiesgo).HasColumnName("idNivelRiesgo");

                entity.Property(e => e.IdPlantilla).HasColumnName("idPlantilla");

                entity.Property(e => e.IdRealizador).HasColumnName("idRealizador");

                entity.Property(e => e.IdRevisador).HasColumnName("idRevisador");

                entity.Property(e => e.IdTipoPlan).HasColumnName("idTipoPlan");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Proyecto)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Realizadopor)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Revisadopor)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdCentroNavigation)
                    .WithMany(p => p.Planes)
                    .HasForeignKey(d => d.IdCentro)
                    .HasConstraintName("FK_Planes_Centros");

                entity.HasOne(d => d.IdClasificacionNavigation)
                    .WithMany(p => p.Planes)
                    .HasForeignKey(d => d.IdClasificacion)
                    .HasConstraintName("FK_Planes_ClasificacionesPlan");

                entity.HasOne(d => d.IdNivelRiesgoNavigation)
                    .WithMany(p => p.Planes)
                    .HasForeignKey(d => d.IdNivelRiesgo)
                    .HasConstraintName("FK_Planes_NivelesRiesgo");

                entity.HasOne(d => d.IdPlantillaNavigation)
                    .WithMany(p => p.Planes)
                    .HasForeignKey(d => d.IdPlantilla)
                    .HasConstraintName("FK_Planes_Plantillas");

                entity.HasOne(d => d.IdRealizadorNavigation)
                    .WithMany(p => p.PlanesIdRealizadorNavigation)
                    .HasForeignKey(d => d.IdRealizador)
                    .HasConstraintName("FK_Planes_Usuarios");

                entity.HasOne(d => d.IdRevisadorNavigation)
                    .WithMany(p => p.PlanesIdRevisadorNavigation)
                    .HasForeignKey(d => d.IdRevisador)
                    .HasConstraintName("FK_Planes_Usuarios1");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.PlanesIdUsuarioNavigation)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_Planes_Usuarios2");
            });

            modelBuilder.Entity<PlanesActividades>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Actividad)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DescripcionTrabajo).IsUnicode(false);

                entity.Property(e => e.DescripcionWord).IsUnicode(false);

                entity.Property(e => e.DetallesAsociados).IsUnicode(false);

                entity.Property(e => e.EvaluacionRiesgos).IsUnicode(false);

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdActividad).HasColumnName("idActividad");

                entity.Property(e => e.IdCapitulo).HasColumnName("idCapitulo");

                entity.Property(e => e.IdEstado).HasColumnName("idEstado");

                entity.Property(e => e.IdPlan).HasColumnName("idPlan");

                entity.Property(e => e.IdSubcapitulo).HasColumnName("idSubcapitulo");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Idaux).HasColumnName("idaux");

                entity.Property(e => e.IdauxPadre).HasColumnName("idauxPadre");

                entity.Property(e => e.MaquinaHerramienta).IsUnicode(false);

                entity.Property(e => e.MediosAuxiliares).IsUnicode(false);

                entity.Property(e => e.OrganizacionTrabajo).IsUnicode(false);

                entity.Property(e => e.Texto)
                    .HasMaxLength(2500)
                    .IsUnicode(false);

                entity.Property(e => e.Titulo)
                    .HasMaxLength(2500)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdPlanNavigation)
                    .WithMany(p => p.PlanesActividades)
                    .HasForeignKey(d => d.IdPlan)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PlanesActividades_Planes1");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.PlanesActividades)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_PlanesActividades_Usuarios");
            });

            modelBuilder.Entity<PlanesDatosParticulares>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EstructuraOrganizativa)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.HayInterySer).HasColumnName("hayInterySer");

                entity.Property(e => e.IdPlan).HasColumnName("idPlan");

                entity.Property(e => e.PlazoEjecucionPrevision).IsUnicode(false);

                entity.Property(e => e.PresupuestoTotal).IsUnicode(false);

                entity.Property(e => e.TipoPlanEmergencia).HasColumnName("tipoPlanEmergencia");

                entity.Property(e => e.Txt1)
                    .HasColumnName("txt1")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Txt10).HasColumnName("txt10");

                entity.Property(e => e.Txt11)
                    .HasColumnName("txt11")
                    .IsUnicode(false);

                entity.Property(e => e.Txt12)
                    .HasColumnName("txt12")
                    .IsUnicode(false);

                entity.Property(e => e.Txt13).HasColumnName("txt13");

                entity.Property(e => e.Txt14)
                    .HasColumnName("txt14")
                    .IsUnicode(false);

                entity.Property(e => e.Txt15)
                    .HasColumnName("txt15")
                    .IsUnicode(false);

                entity.Property(e => e.Txt2)
                    .HasColumnName("txt2")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Txt3).HasColumnName("txt3");

                entity.Property(e => e.Txt4).HasColumnName("txt4");

                entity.Property(e => e.Txt5).HasColumnName("txt5");

                entity.Property(e => e.Txt6)
                    .HasColumnName("txt6")
                    .IsUnicode(false);

                entity.Property(e => e.Txt7)
                    .HasColumnName("txt7")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Txt8).HasColumnName("txt8");

                entity.Property(e => e.Txt9).HasColumnName("txt9");

                entity.HasOne(d => d.IdPlanNavigation)
                    .WithMany(p => p.PlanesDatosParticulares)
                    .HasForeignKey(d => d.IdPlan)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PlanesDatosParticulares_Planes");
            });

            modelBuilder.Entity<PlanesPlanos>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Archivo)
                    .HasColumnName("archivo")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdPlan).HasColumnName("idPlan");

                entity.Property(e => e.IdPlanoGeneral).HasColumnName("idPlanoGeneral");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdPlanoGeneralNavigation)
                    .WithMany(p => p.PlanesPlanos)
                    .HasForeignKey(d => d.IdPlanoGeneral)
                    .HasConstraintName("FK_PlanesPlanos_Planes");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.PlanesPlanos)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_PlanesPlanos_Usuarios");
            });

            modelBuilder.Entity<PlanesPresupuestos>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdArticulo).HasColumnName("idArticulo");

                entity.Property(e => e.IdMoneda).HasColumnName("idMoneda");

                entity.Property(e => e.IdPlan).HasColumnName("idPlan");

                entity.Property(e => e.IdTipoArticulo).HasColumnName("idTipoArticulo");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.NumUsos).HasColumnName("numUsos");

                entity.HasOne(d => d.IdMonedaNavigation)
                    .WithMany(p => p.PlanesPresupuestos)
                    .HasForeignKey(d => d.IdMoneda)
                    .HasConstraintName("FK_PlanesPresupuestos_Monedas");

                entity.HasOne(d => d.IdPlanNavigation)
                    .WithMany(p => p.PlanesPresupuestos)
                    .HasForeignKey(d => d.IdPlan)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PlanesPresupuestos_Planes");

                entity.HasOne(d => d.IdTipoArticuloNavigation)
                    .WithMany(p => p.PlanesPresupuestos)
                    .HasForeignKey(d => d.IdTipoArticulo)
                    .HasConstraintName("FK_PlanesPresupuestos_Articulos");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.PlanesPresupuestos)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_PlanesPresupuestos_Usuarios");
            });

            modelBuilder.Entity<PlanosGenerales>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Archivo)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdFamilia).HasColumnName("idFamilia");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdFamiliaNavigation)
                    .WithMany(p => p.PlanosGenerales)
                    .HasForeignKey(d => d.IdFamilia)
                    .HasConstraintName("FK_PlanosGenerales_Familias");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.PlanosGenerales)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_PlanosGenerales_Usuarios");
            });

            modelBuilder.Entity<Plantillas>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Archivo)
                    .HasColumnName("archivo")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Plantilla)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Plantillas)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_Plantillas_Usuarios");
            });

            modelBuilder.Entity<ProbabilidadesRiesgo>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Codigo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Orden).HasColumnName("orden");

                entity.Property(e => e.Probabilidad)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.ProbabilidadesRiesgo)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_ProbabilidadesRiesgo_Usuarios");
            });

            modelBuilder.Entity<Riesgos>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdActividadRiesgo).HasColumnName("idActividadRiesgo");

                entity.Property(e => e.IdEstado).HasColumnName("idEstado");

                entity.Property(e => e.IdNuevo).HasColumnName("idNuevo");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Riesgo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Riesgos)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_Riesgos_Usuarios");
            });

            modelBuilder.Entity<Subcapitulos>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descripcion).IsUnicode(false);

                entity.Property(e => e.DescripcionTrabajo).IsUnicode(false);

                entity.Property(e => e.DetallesAsociados).IsUnicode(false);

                entity.Property(e => e.EvaluacionRiesgos).IsUnicode(false);

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdCapitulo).HasColumnName("idCapitulo");

                entity.Property(e => e.IdEstado).HasColumnName("idEstado");

                entity.Property(e => e.IdSubcapituloAnt).HasColumnName("idSubcapituloAnt");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.MaquinaHerramienta).IsUnicode(false);

                entity.Property(e => e.MediosAuxiliares).IsUnicode(false);

                entity.Property(e => e.OrganizacionTrabajo).IsUnicode(false);

                entity.Property(e => e.Titulo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Subcapitulos)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_Subcapitulos_Usuarios");
            });

            modelBuilder.Entity<TiposArticulo>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.TipoArticulo)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.TiposArticulo)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_TiposArticulo_Usuarios");
            });

            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Clave)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EMail)
                    .HasColumnName("eMail")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fecCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fecModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdCargo).HasColumnName("idCargo");

                entity.Property(e => e.IdDepartamento).HasColumnName("idDepartamento");

                entity.Property(e => e.IdDireccion).HasColumnName("idDireccion");

                entity.Property(e => e.IdTipoUsu).HasColumnName("idTipoUsu");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Tel)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Usuario)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdCargoNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdCargo)
                    .HasConstraintName("FK_Usuarios_Cargos");

                entity.HasOne(d => d.IdDepartamentoNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdDepartamento)
                    .HasConstraintName("FK_Usuarios_Departamentos");

                entity.HasOne(d => d.IdDireccionNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdDireccion)
                    .HasConstraintName("FK_Usuarios_DireccionesNegocio");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.InverseIdUsuarioNavigation)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_Usuarios_Usuarios1");
            });
        }
    }
}
