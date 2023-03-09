using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.DataAccessManagers.DamBase;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.DataAccessLayer.Enums;

namespace Segurplan.DataAccessLayer.DataAccessManagers {
    public class TemplateDam : SegurplanDamBase {
        public const string OrderById = "Id";
        public const string OrderByName = "Name";
        public const string OrderByNotes = "Notes";
        public const string OrderByCreatedBy = "CreatedBy";
        public const string OrderByModifiedDate = "ModifiedDate";
        public const string FilterById = "Id";
        public const string FilterByName = "Name";
        public const string FilterByNotes = "Notes";
        public const string FilterByCreatedBy = "CreatedBy";
        public const string FilterByModifiedDate = "ModifiedDate";

        public TemplateDam(SegurplanContext context) : base(context) {
        }

        public List<Template> TemplateList() {
            try {
                return (from x in context.Template select x).ToList();
            } catch (Exception e) {
                Debug.WriteLine($"Error loading template list :: {e.ToString()}");

                return new List<Template>(0);
            }
        }        

        public int CreateTemplate(Template template) {
            context.Template.Add(template);
            return context.SaveChanges() > 0 ? template.Id : -1;
        }

        public IQueryable<Template> SelectAll() => (from m in context.Template
                                                                         select new Template {
                                                                             Id = m.Id,
                                                                             Name = m.Name,
                                                                             Notes = m.Notes,
                                                                             CreatedBy = m.CreatedBy,
                                                                             CreateDate = m.CreateDate,
                                                                             ModifiedBy = m.ModifiedBy,
                                                                             UpdateDate = m.UpdateDate,
                                                                             FilePath = m.FilePath
                                                                         }
        );

        public IQueryable<Template> ApplyFilters(IQueryable<Template> query, string filterName, string filterValue) {
            switch (filterName) {
                case FilterById:
                    return (from x in query where (Convert.ToInt32(x.Id)) == (Convert.ToInt32(filterValue)) select x);

                case FilterByName:
                    return (from x in query where x.Name.Contains(filterValue) select x);

                case FilterByNotes:
                    return (from x in query where x.Notes.Contains(filterValue) select x);

                case FilterByCreatedBy:
                    var userId = (from m in context.Users where m.CompleteName.Contains(filterValue) select m.Id).FirstOrDefault();
                    return (from x in query where x.CreatedBy == userId select x);

                case FilterByModifiedDate:
                    return (from x in query where x.UpdateDate.ToShortDateString() == filterValue select x);

                default:
                    return (from x in query select x);
            }
        }

        public IQueryable<Template> GetListOffset(IQueryable<Template> query, int IndexPage, int PageRows, int Takep) {
            return (from x in query select x).Skip(IndexPage * PageRows).Take(Takep);
        }

        public IQueryable<Template> OrderList(IQueryable<Template> query, string orderBy, bool orderModeDesc) {
            switch (orderBy) {
                case OrderById:
                    return orderModeDesc
                       ? (from x in query select x).OrderByDescending(x => (Convert.ToInt32(x.Id)))
                       : (from x in query select x).OrderBy(x => (Convert.ToInt32(x.Id)));


                case OrderByName:
                    return orderModeDesc
                         ? (from x in query select x).OrderByDescending(x => x.Name)
                         : (from x in query select x).OrderBy(x => x.Name);


                default:
                    return orderModeDesc
                       ? (from x in query select x).OrderByDescending(x => (Convert.ToInt32(x.Id)))
                       : (from x in query select x).OrderBy(x => (Convert.ToInt32(x.Id)));

            }
        }

        public Template UpdateTemplate(Template dbTemplate) {
            context.Template.Update(dbTemplate);
            var saveOk = context.SaveChanges() > 0 ? true : false;
            if (saveOk) {
                return dbTemplate;
            } else {
                return null;
            }
        }

        public Template SelectByTemplateId(int templateId) {
            return (from x in context.Template where x.Id == templateId select new Template {
                Id = x.Id,
                Name = x.Name,
                Notes = x.Notes,
                FilePath = x.FilePath,
                CreatedBy = x.CreatedBy,
                CreateDate = x.CreateDate,
                ModifiedBy = x.ModifiedBy,
                UpdateDate = x.UpdateDate,
                FileSize = x.FileSize,
                TemplateType = x.TemplateType
            }).FirstOrDefault();
        }

        public Template SelectByTemplateName(string templateName) {
            return (from x in context.Template
                    where x.Name == templateName
                    select new Template {
                        Id = x.Id,
                        Name = x.Name,
                        Notes = x.Notes,
                        FilePath = x.FilePath,
                        CreatedBy = x.CreatedBy,
                        CreateDate = x.CreateDate,
                        ModifiedBy = x.ModifiedBy,
                        UpdateDate = x.UpdateDate,
                        FileData = x.FileData,
                        FileSize = x.FileSize
                    }).FirstOrDefault();
        }

        public bool DeleteTemplateAsync(int templateId) {
            var measure = (from x in context.Template where x.Id == templateId select x).FirstOrDefault();
            context.Template.Remove(measure);

            return  context.SaveChanges() > 0 ? true : false;
        }
    }
}
