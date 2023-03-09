using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OldDBDataMigrator.ProduccionDBModels;
using OldDBDataMigrator.Utils;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace OldDBDataMigrator.DataMigration.DuplicateRiskAssignments {
    public class DuplicateRiskAssignments {
        private readonly SegurplanContext segurplanContext;
        private readonly SeedUtils utils;

        public DuplicateRiskAssignments(SegurplanContext segurplanContext, SeedUtils utils) {
            this.segurplanContext = segurplanContext;
            this.utils = utils;
        }
        public async Task Initialize() {
            bool validKey = false;

            while (!validKey) {
                string userInput = utils.PrintMessageAndReadLine(
                    "\rEscriba el Id del capitulo del que desea pasar sus asignaciones de version vigente a las asignaciones de version borrador");

                validKey = int.TryParse(userInput, out int result);
                if (result != null && result != 0) {
                    await DuplicateData(result);
                } else {
                    utils.PrintErrorMessage("Comando no válido");
                }
            }
        }
        public async Task DuplicateData(int chapterId) {
            try {
                var chapterVersionList = await segurplanContext.ChapterVersion
                                         .Where(x => x.IdChapter == chapterId)
                                         .ToListAsync().ConfigureAwait(true);
                var currentChapterVersion = chapterVersionList.Where(x => x.VersionNumber == chapterVersionList.Count - 1).FirstOrDefault();
                if (currentChapterVersion != null) {

                    var subchapterVersionList = await segurplanContext.SubChapterVersion.Where(x => x.IdChapterVersion == currentChapterVersion.Id).ToListAsync().ConfigureAwait(true);
                    var subchapterVersionIdList = subchapterVersionList.Select(x => x.Id).ToList();

                    var actVersionList = await segurplanContext.ActivityVersion.Where(x => subchapterVersionIdList.Contains(x.IdSubChapterVersion)).ToListAsync().ConfigureAwait(true);
                    var actVersionIdList = actVersionList.Select(x => x.IdActivity).ToList();

                    var borradorChapterVersion = chapterVersionList.Where(x => x.VersionNumber == chapterVersionList.Count).FirstOrDefault();

                    var borradorSubchapterVersionList = await segurplanContext.SubChapterVersion.Where(x => x.IdChapterVersion == borradorChapterVersion.Id).ToListAsync().ConfigureAwait(true);
                    var borradorSubchapterVersionIdList = borradorSubchapterVersionList.Select(x => x.IdSubChapter).ToList();

                    var borradorActivityVersionList = await segurplanContext.ActivityVersion.Where(x => borradorSubchapterVersionIdList.Contains(x.IdSubChapterVersion)).ToListAsync().ConfigureAwait(true);

                    var RiskAsignmentsCurrentChapterVersion = await segurplanContext.RisksAndPreventiveMeasures
                    .Where(x => actVersionIdList.Contains(x.ActivityId)).ToListAsync().ConfigureAwait(true);
                    var RiskAsignmentsCurrentChapterVersionIds = RiskAsignmentsCurrentChapterVersion.Select(x => x.Id).ToList();
                    var RiskAndPreventiveMeasuresAssignment = await segurplanContext.RiskAndPreventiveMeasuresMeasures.Where(x => RiskAsignmentsCurrentChapterVersionIds.Contains(x.RisksAndPreventiveMeasuresId)).ToListAsync();
                    foreach (var riskAssignment in RiskAsignmentsCurrentChapterVersion) {
                        var numberSubchap = subchapterVersionList.Where(x => x.IdSubChapter == riskAssignment.SubChapterId).Select(x => x.Number).FirstOrDefault();
                        var numberAct = actVersionList.Where(x => x.IdActivity == riskAssignment.ActivityId).Select(x => x.Number).FirstOrDefault();

                        var newSubchapterId = borradorSubchapterVersionList.Where(x => x.Number == numberSubchap).Select(x => x.IdSubChapter).FirstOrDefault();
                        var newSubchapterVersionId = borradorSubchapterVersionList.Where(x => x.Number == numberSubchap).Select(x => x.Id).FirstOrDefault();
                        var newActivityId = borradorActivityVersionList.Where(x => x.IdSubChapterVersion == newSubchapterVersionId && x.Number == numberAct).Select(x => x.IdActivity).FirstOrDefault();
                        if (newSubchapterId != 0 && newActivityId != 0) {
                            riskAssignment.ActivityId = newActivityId;
                            riskAssignment.SubChapterId = newSubchapterId;
                            riskAssignment.CreateDate = DateTime.Now;
                            riskAssignment.UpdateDate = DateTime.Now;
                            var RiskAndPreventiveMeasureMeasures = RiskAndPreventiveMeasuresAssignment.Where(x => x.RisksAndPreventiveMeasuresId == riskAssignment.Id).ToList();
                            riskAssignment.Id = 0;
                            foreach (var riskAndPrevMeasureMeasure in RiskAndPreventiveMeasureMeasures) {
                                segurplanContext.RiskAndPreventiveMeasuresMeasures.Add(new RiskAndPreventiveMeasuresMeasures { RisksAndPreventiveMeasures = riskAssignment, PreventiveMeasureId= riskAndPrevMeasureMeasure.PreventiveMeasureId, PreventiveMeasureOrder = riskAndPrevMeasureMeasure.PreventiveMeasureOrder });
                            }
                        }
                    }
                    await segurplanContext.SaveChangesAsync();
                } else {
                    utils.PrintErrorMessage("El Id introducido no coincide con ningun capitulo");
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }

    }
}
