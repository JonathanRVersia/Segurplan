using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.DataAccessLayer.Enums;

namespace OldDBDataMigrator.DataMigration.Actions {
    public class E_TemplatesSeed : ISeedInitializer {

        private readonly SegurplanContext segurplanContext;

        public E_TemplatesSeed(SegurplanContext segurplanContext) {
            this.segurplanContext = segurplanContext;
        }

        private Dictionary<string, string> fileNotesDictionary = new Dictionary<string, string> {
            { "Plan formato sin tablas completo.docx","Plantillas para generar planes sin tablas" },
            { "Plan nuevo formato tabla.docx","Plantilla para generar planes con el nuevo formato de tablas" },
            { "Plan SS corto EBS.docx","Plantillas para generar planes en formato corto EBS" },
            { "Plan SS corto ESS.docx","Plantillas para generar planes en formato corto ESS" },
            { "Plan SS largo EBS 1.docx","Plantillas para generar planes en formato largo EBS1" },
            { "Plan SS largo ESS 1.docx","Plantillas para generar planes en formato largo ESS1" },
            { "ER nuevo formato tabla 2 SOLO CAP.docx","Plantilla para generar evaluaciones de riesgo por separado" },
            { "Evaluacion de riesgos.docx","Plantilla para generar evaluaciones de riesgo conjuntas" }
        };

        private Dictionary<string, TemplateType> fileTypesDictionary = new Dictionary<string, TemplateType> {
            { "Plan formato sin tablas completo.docx",TemplateType.PlanManagement},
            { "Plan nuevo formato tabla.docx",TemplateType.PlanManagement },
            { "Plan SS corto EBS.docx",TemplateType.PlanManagement},
            { "Plan SS corto ESS.docx",TemplateType.PlanManagement },
            { "Plan SS largo EBS 1.docx",TemplateType.PlanManagement },
            { "Plan SS largo ESS 1.docx",TemplateType.PlanManagement },
            { "ER nuevo formato tabla 2 SOLO CAP.docx",TemplateType.RiskAndPreventiveMeasures },
            { "Evaluacion de riesgos.docx",TemplateType.RiskAndPreventiveMeasures}
        };

        private string defaultTemplateString = "Plan SS largo ESS 1.docx";

        private List<FileInfo> fileInfos = new List<FileInfo>();
        private List<Template> templates = new List<Template>();

        public async Task Seed() {
            await GetOriginalData();
            Convert();
            await SetDestinationData();
        }

        public Task GetOriginalData() {

            var fileNames = Directory.GetFiles("Templates");

            foreach (var fileName in fileNames) {
                fileInfos.Add(new FileInfo(fileName));
            }

            return Task.CompletedTask;
        }

        public void Convert() {

            foreach (var fileInfo in fileInfos) {
                templates.Add(ConvertToTemplate(fileInfo));
            }
        }

        public async Task SetDestinationData() {
            if (templates.Any()) {
                SetDefaultPosition();

                await segurplanContext.AddRangeAsync(templates);

                int changes = await segurplanContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Se usa porque en el SQL el id del template en SafetyStufyPlan apunta al Template.Id 5.
        /// Lo dejamos en el index cuatro así le generara el Id 5
        /// </summary>
        private void SetDefaultPosition() {
            var defaultTemplate = templates.Where(x => x.FilePath == defaultTemplateString).FirstOrDefault();

            templates.Remove(defaultTemplate);

            templates.Insert(4, defaultTemplate);
        }

        private Template ConvertToTemplate(FileInfo fileInfo) => new Template {
            CreateDate = fileInfo.CreationTime,
            CreatedBy = 1,//Harcoded
            FileData = File.ReadAllBytes(fileInfo.FullName),
            FilePath = fileInfo.Name,
            FileSize = fileInfo.Length,
            ModifiedBy = 1,//Harcoded
            Name = fileInfo.Name.Replace(fileInfo.Extension, ""),
            Notes = fileNotesDictionary.ContainsKey(fileInfo.Name) ?
                            fileNotesDictionary.GetValueOrDefault(fileInfo.Name) :
                            fileInfo.Name.Replace(fileInfo.Extension, ""),//Si no aparece en el diccionario metemos el nombre sin extensión
            UpdateDate = fileInfo.CreationTime,
            TemplateType = fileTypesDictionary.ContainsKey(fileInfo.Name) ?
                            fileTypesDictionary.GetValueOrDefault(fileInfo.Name) : TemplateType.NoType,//Si no aparece en el diccionario metemos el nombre sin extensión
        };
    }
}
