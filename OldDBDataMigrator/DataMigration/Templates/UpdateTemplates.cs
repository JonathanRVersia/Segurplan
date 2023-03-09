using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OldDBDataMigrator.Utils;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.DataAccessLayer.Enums;

namespace OldDBDataMigrator.DataMigration.Templates {
    public class UpdateTemplates {

        private readonly SegurplanContext segurplanContext;
        private readonly SeedUtils utils;

        private List<Template> templates = new List<Template>();

        public UpdateTemplates(SegurplanContext segurplanContext, SeedUtils utils) {
            this.segurplanContext = segurplanContext;
            this.utils = utils;
        }

        public async Task Initialize() {

            int updatedFiles = 0;

            var fileNames = Directory.GetFiles("Templates");

            foreach (var fileName in fileNames) {
                var fileInfo = new FileInfo(fileName);

                var template = await segurplanContext.Template.Where(x => x.FilePath == fileInfo.Name).FirstOrDefaultAsync();

                if (template != null) {

                    template.FileData = File.ReadAllBytes(fileInfo.FullName);
                    template.FileSize = fileInfo.Length;
                    template.UpdateDate = DateTime.Now;
                    template.ModifiedBy = 1;

                    segurplanContext.Update(template);
                    updatedFiles++;
                } else {
                    templates.Add(ConvertToTemplate(fileInfo));
                }
            }

            if (templates.Any())
                await segurplanContext.AddRangeAsync(templates);

            var changes = await segurplanContext.SaveChangesAsync();

            if (changes > 0)
                utils.PrintSuccessMessage($"Templates actualizados con éxito, {updatedFiles} actualizados y {templates.Count} añadidos");
        }

        private Template ConvertToTemplate(FileInfo fileInfo) => new Template {
            CreateDate = fileInfo.CreationTime,
            CreatedBy = 1,//Harcoded
            FileData = File.ReadAllBytes(fileInfo.FullName),
            FilePath = fileInfo.Name,
            FileSize = fileInfo.Length,
            ModifiedBy = 1,//Harcoded
            Name = fileInfo.Name.Replace(fileInfo.Extension, ""),
            Notes = fileInfo.Name.Replace(fileInfo.Extension, ""),
            UpdateDate = fileInfo.CreationTime,
            TemplateType = TemplateType.NoType,
        };
    }
}
