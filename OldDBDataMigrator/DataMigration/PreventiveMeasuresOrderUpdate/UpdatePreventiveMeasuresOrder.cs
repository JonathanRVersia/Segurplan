using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OldDBDataMigrator.ProduccionDBModels;
using OldDBDataMigrator.Utils;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace OldDBDataMigrator.DataMigration.PreventiveMeasuresOrderUpdate {
    public class UpdatePreventiveMeasuresOrder {
        private readonly IMapper mapper;
        private readonly SegurplanContext segurplanContext;
        private readonly SegurplanProduccionContext produccionContext;
        private readonly SeedUtils utils;

        public List<Medidas> medidas { get; private set; }
        public List<EvaluacionesMedida> evaluacionesMedidas { get; private set; }
        public List<RiskAndPreventiveMeasuresMeasures> riskAndPreventiveMeasuresMeasures { get; private set; }

        public UpdatePreventiveMeasuresOrder(IMapper mapper, SegurplanContext segurplanContext, SegurplanProduccionContext produccionContext, SeedUtils utils) {
            this.mapper = mapper;
            this.segurplanContext = segurplanContext;
            this.produccionContext = produccionContext;
            this.utils = utils;
        }
        public async Task Initialize() {
            await GetOriginalData();
        }
        public async Task GetOriginalData() {
            try {

                medidas = await produccionContext.Medidas.Where(x => x.IdEstado == 2).ToListAsync();
                evaluacionesMedidas = await produccionContext.EvaluacionesMedida.Where(x => x.IdEstado == 2)
                    .Include(y => y.IdMedidaNavigation)
                    .Include(y => y.IdRiesgoNavigation)
                    .Include(y => y.IdCapituloNavigation)
                    .Include(y => y.IdSubcapituloNavigation)
                    .Include(y => y.IdActividadNavigation)
                    .ToListAsync();
                riskAndPreventiveMeasuresMeasures = await segurplanContext.RiskAndPreventiveMeasuresMeasures.Include(x => x.PreventiveMeasure)
                    .Include(x => x.RisksAndPreventiveMeasures).ThenInclude(z => z.Risk)
                    .Include(x => x.RisksAndPreventiveMeasures).ThenInclude(z => z.Activity)
                    .Include(x => x.RisksAndPreventiveMeasures).ThenInclude(z => z.Chapter)
                    .Include(x => x.RisksAndPreventiveMeasures).ThenInclude(z => z.SubChapter)
                    .ToListAsync();
                segurplanContext.ChangeTracker.AutoDetectChangesEnabled = false;
                foreach (var measureRisk in riskAndPreventiveMeasuresMeasures) {
                    var riskAndPreventiveMeasures = evaluacionesMedidas.Where(x => x.IdRiesgoNavigation?.Codigo == measureRisk.RisksAndPreventiveMeasures.Risk.Code
                    && x.IdMedidaNavigation?.Codigo == measureRisk.PreventiveMeasure.Code
                    && x.IdCapituloNavigation?.Capitulo == measureRisk.RisksAndPreventiveMeasures.Chapter.Number
                    && x.IdSubcapituloNavigation?.SubCapitulo == measureRisk.RisksAndPreventiveMeasures.SubChapter.Number
                    && x.IdActividadNavigation.Actividad.Contains(measureRisk.RisksAndPreventiveMeasures.Activity.Number.ToString()))
                        .LastOrDefault();
                    if (riskAndPreventiveMeasures != null) {
                        if (riskAndPreventiveMeasures.OrdenMedida != 0) {
                            segurplanContext.Entry(measureRisk.PreventiveMeasure).State = EntityState.Unchanged;
                            segurplanContext.Entry(measureRisk.RisksAndPreventiveMeasures).State = EntityState.Unchanged;
                            segurplanContext.Entry(measureRisk).Property(x=>x.PreventiveMeasureOrder).IsModified = true;
                            measureRisk.PreventiveMeasureOrder = riskAndPreventiveMeasures.OrdenMedida ?? 0;

                        }
                    }
                }
                segurplanContext.ChangeTracker.DetectChanges();
                await segurplanContext.SaveChangesAsync();
            } catch (Exception e){
                Console.WriteLine(e.Message);
            } finally {
                segurplanContext.ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }

    }
}
