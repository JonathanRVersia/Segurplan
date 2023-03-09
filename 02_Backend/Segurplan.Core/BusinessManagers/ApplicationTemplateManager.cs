using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.DataAccessManagers;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Core.BusinessManagers {

    public class ApplicationTemplateManager {
        private readonly UserDam usrDam;
        private readonly TemplateDam templateDam;

        public ApplicationTemplateManager(TemplateDam templateDam, UserDam usrDam) {
            this.usrDam = usrDam;
            this.templateDam = templateDam;
        }

        public async Task<int> CreateTemplate(ApplicationTemplate template, int userId) {
            try {
                var dbTemplate = ToTemplate(template);
                if (dbTemplate == null)
                    return -1;

                dbTemplate.CreatedBy = userId;
                dbTemplate.CreateDate = DateTime.Now;
                dbTemplate.ModifiedBy = userId;

                if(template.FileDetails != null) {
                    dbTemplate.FilePath = template.FileDetails.Name;
                    dbTemplate.FileData = template.FileData;
                    dbTemplate.FileSize = Convert.ToDecimal(template.FileDetails.FileSize);
                }       
                
                return templateDam.CreateTemplate(dbTemplate);
            } catch (Exception e) {
                Debug.WriteLine(e.ToString());
                return -1;
            }
        }

        public ApplicationTemplate UpdateTemplate(ApplicationTemplate template, int userId) {
            try {
                var dbTemplate = templateDam.SelectByTemplateId(template.Id);
                var usr = usrDam.SelectUserById(userId);

                dbTemplate.UpdateDate = DateTime.Now;
                dbTemplate.Name = template.Name;
                dbTemplate.Notes = template.Notes;
                dbTemplate.ModifiedBy = usr.Id;
                dbTemplate.TemplateType = template.TemplateType;

                if(template.Archivo != null) {
                    dbTemplate.FilePath = template.Archivo.FileName;
                    dbTemplate.FileData = template.FileData;
                    dbTemplate.FileSize = template.Archivo.Length;
                }

                return ToApplicationTemplate(templateDam.UpdateTemplate(dbTemplate));
            } catch (Exception e) {
                Debug.WriteLine(e.ToString());
                return null;
            }
        }

        public ApplicationTemplate TemplateData(int templateId) {
            try {
                var response = ToApplicationTemplate(templateDam.SelectByTemplateId(templateId));

                return response;
            } catch (Exception e) {
                Debug.WriteLine(e.ToString());
                return null;
            }
        }

        public async Task<bool> DeleteTemplate(int templateId) {
            try {
                return templateDam.DeleteTemplateAsync(templateId);
            } catch (Exception e) {
                Debug.WriteLine(e.ToString());
                return false;
            }
        }

        private ApplicationTemplate ToApplicationTemplate(Template template) {
            try {
                var createdBy = usrDam.SelectUserById(template.CreatedBy);
                var modifiedBy = usrDam.SelectUserById(template.ModifiedBy);

                return new ApplicationTemplate() {
                    Id = template.Id,
                    Name = template.Name,
                    Notes = template.Notes,
                    CreationDate = string.IsNullOrEmpty(template.CreateDate.ToString()) ? string.Empty : template.CreateDate.ToString(),
                    CreatedBy = template.CreatedBy,
                    CreatorName = createdBy.CompleteName,
                    ModifiedDate = string.IsNullOrEmpty(template.UpdateDate.ToString()) ? string.Empty : template.UpdateDate.ToString(),
                    ModifiedBy = template.ModifiedBy,
                    ModifiedByName = modifiedBy.CompleteName,
                    FileData = template.FileData,
                    FilePath = template.FilePath,
                    FileSize = template.FileSize,
                    TemplateType = template.TemplateType
                };
            } catch (Exception e) {
                Debug.WriteLine(e.ToString());
                return null;
            }
        }

        private Template ToTemplate(ApplicationTemplate template) {
            try {
                return new Template() {
                    Id = template.Id,
                    Name = template.Name,
                    Notes = template.Notes,
                    FilePath = template.Archivo.FileName,
                    TemplateType = template.TemplateType,
                    FileData = template.FileData,
                    FileSize = template.Archivo.Length
                };
            } catch (Exception e) {
                Debug.WriteLine(e.ToString());
                return null;
            }
        }
    }
}





