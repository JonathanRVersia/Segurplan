using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Segurplan.Core.Actions.Administration.Templates;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.DataAccessManagers;
using Segurplan.Core.Helpers;

namespace Segurplan.Core.BusinessManagers {

    public class TemplateListManager {
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
        public bool OrderModeDesc = false;

        public List<ApplicationTemplate> Response { get; set; } = new List<ApplicationTemplate>();
        public TemplateListTableState TableState { get; set; }
        public TemplatesFilter TableFilter { get; set; }
        public int FilteredTemplates { get; internal set; }
        public int IndexPage { get; internal set; }
        public int RemainingRows { get; internal set; }
        public int Takep { get; internal set; }
        private readonly TemplateDam templateDam;
        private readonly UserDam usrDam;

        public TemplateListManager(TemplateDam templateDam, UserDam usrDam) {
            this.templateDam = templateDam;
            this.usrDam = usrDam;
        }

        public List<ApplicationTemplate> GetTemplateList(TemplateListTableState tableState, TemplatesFilter tableFilter) {
            try {
                TableState = tableState;
                TableFilter = tableFilter;

                var dbResponse = templateDam.SelectAll();
                dbResponse = string.IsNullOrEmpty(TableFilter.Id) ? dbResponse : templateDam.ApplyFilters(dbResponse, FilterById, TableFilter.Id);
                dbResponse = string.IsNullOrEmpty(TableFilter.Name) ? dbResponse : templateDam.ApplyFilters(dbResponse, FilterByName, TableFilter.Name);
                dbResponse = string.IsNullOrEmpty(TableFilter.Notes) ? dbResponse : templateDam.ApplyFilters(dbResponse, FilterByNotes, TableFilter.Notes);
                dbResponse = string.IsNullOrEmpty(TableFilter.CreatedBy) ? dbResponse : templateDam.ApplyFilters(dbResponse, FilterByCreatedBy, TableFilter.CreatedBy);
                dbResponse = string.IsNullOrEmpty(TableFilter.ModifiedDate) ? dbResponse : templateDam.ApplyFilters(dbResponse, FilterByModifiedDate, TableFilter.ModifiedDate);
                //After filtering we need to count results
                FilteredTemplates = dbResponse.Count();                
                IndexPage = tableState.IndexPage;
                if (tableState.OrderModeDesc == Helpers.ListOrderMode.Desc) { OrderModeDesc = true; }
                dbResponse = templateDam.OrderList(dbResponse, tableState.OrderBy, OrderModeDesc);
                
                //check if enought rows remaining to fill the response
                RemainingRows = FilteredTemplates - (tableState.PageRows * IndexPage);
                Takep = tableState.PageRows;
                if (RemainingRows - tableState.PageRows < 0) {
                    Takep = RemainingRows;
                }
                if (RemainingRows == 0) {
                    //not enough rows to fill this page, lets see if enough row to fill indexPage - 1 because we will check planlist.count() later
                    IndexPage = IndexPage - 1;
                }
                if (dbResponse.Count() > 0) {
                    dbResponse = templateDam.GetListOffset(dbResponse, IndexPage, tableState.PageRows, Takep);
                }
                                
                var response = new List<ApplicationTemplate>();
                foreach (var dbTemplate in dbResponse) {
                    var createdBy = usrDam.SelectUserById(dbTemplate.CreatedBy);
                    var modifiedBy = usrDam.SelectUserById(dbTemplate.ModifiedBy);

                    response.Add(new ApplicationTemplate {
                        Id = dbTemplate.Id,
                        Name = dbTemplate.Name,
                        Notes = dbTemplate.Notes,
                        CreatedBy = dbTemplate.CreatedBy,
                        CreatorName = createdBy.CompleteName,
                        ModifiedDate = dbTemplate.UpdateDate.ToShortDateString()
                    });
                }
                //ApplicationTemplate
                return response;

            } catch (Exception e) {
                Debug.WriteLine(e.ToString());
                return null;
            }
        }
    }
}

