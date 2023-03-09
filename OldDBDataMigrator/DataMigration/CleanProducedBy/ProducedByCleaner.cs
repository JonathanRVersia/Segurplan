using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OldDBDataMigrator.Utils;
using Segurplan.Core.Database;

namespace OldDBDataMigrator.DataMigration.CleanProducedBy {
    public class ProducedByCleaner {
        private readonly SegurplanContext segurplanContext;
        private readonly SeedUtils utils;

        public ProducedByCleaner(SegurplanContext segurplanContext, SeedUtils utils) {
            this.segurplanContext = segurplanContext;
            this.utils = utils;
        }

        public async Task Initialize() {
            var groupedList = await segurplanContext.UserChapterVersion.GroupBy(x => new { x.ChapterVersionId, x.UserId }).ToListAsync();

            var groupedDuplicatedElements = groupedList.Where(x => x.Count() > 1).ToList();

            foreach (var group in groupedDuplicatedElements) {
                var chapterUserList = group.ToList();
                bool isFirstElement = true;

                foreach (var chapterUser in chapterUserList) {
                    if (!isFirstElement)
                        segurplanContext.Remove(chapterUser);

                    isFirstElement = false;
                }
            }

            var itemsDeleted = await segurplanContext.SaveChangesAsync();
            utils.PrintSuccessMessage($"{itemsDeleted} elementos duplicados eliminados con éxito");
        }
    }
}
