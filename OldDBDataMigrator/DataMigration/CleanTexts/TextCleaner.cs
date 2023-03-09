using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;

namespace OldDBDataMigrator.DataMigration.CleanTexts {
    public class TextCleaner {

        private readonly SegurplanContext segurplanContext;

        public TextCleaner(SegurplanContext segurplanContext) {
            this.segurplanContext = segurplanContext;
        }

        public async Task Initialize() {
            await CleanPreventiveMeasuresDescription();
        }

        private async Task CleanPreventiveMeasuresDescription() {
            var preventiveMeasures = await segurplanContext.PreventiveMeasure.ToListAsync();

            foreach (var preventiveMeasure in preventiveMeasures) {
                preventiveMeasure.Description = preventiveMeasure.Description
                    .Replace("<", "&lt;")
                    .Replace(">", "&gt;");
            }

            segurplanContext.UpdateRange(preventiveMeasures);
            await segurplanContext.SaveChangesAsync();
        }
    }
}
