using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Segurplan.Core.Actions.Administration.Family;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.DataAccessManagers;

namespace Segurplan.Core.BusinessManagers {
    public class FamilyListManager {
        public const string OrderById = "Id";
        public const string OrderByFamily = "Family";
        public const string FilterById = "Id";
        public const string FilterByFamily = "Family";
        public bool OrderModeDesc = false;

        public List<ApplicationFamily> Response { get; set; } = new List<ApplicationFamily>();
        public FamilyListTableState TableState { get; set; }
        public FamilyFilter TableFilter { get; set; }
        public int FilteredFamily { get; internal set; }
        public int IndexPage { get; internal set; }
        public int RemainingRows { get; internal set; }
        public int Takep { get; internal set; }

        private readonly FamilyDam familyDam;

        public FamilyListManager(FamilyDam familyDam) {
            this.familyDam = familyDam;
        }

        public async Task<List<ApplicationFamily>> GetFamilyList(FamilyListTableState tableState, FamilyFilter tableFilter) {
            try 
            {
                TableState = tableState;
                TableFilter = tableFilter;

                var dbResponse = await familyDam.SelectAll();

                if (!string.IsNullOrEmpty(TableFilter.Id)) {

                    dbResponse = await familyDam.ApplyFilters(dbResponse, FilterById, TableFilter.Id);

                }
                dbResponse = string.IsNullOrEmpty(TableFilter.Family) ? dbResponse : await familyDam.ApplyFilters(dbResponse, FilterByFamily, TableFilter.Family);

                FilteredFamily = dbResponse.Count();
                IndexPage = tableState.IndexPage;
                if (tableState.OrderModeDesc == Helpers.ListOrderMode.Desc) { OrderModeDesc = true; }
                dbResponse = await familyDam.OrderList(dbResponse, tableState.OrderBy, OrderModeDesc);

                RemainingRows = FilteredFamily - (tableState.PageRows * IndexPage);
                Takep = tableState.PageRows;
                if (RemainingRows - tableState.PageRows < 0) {
                    Takep = RemainingRows;
                }
                if (RemainingRows == 0) {
                    IndexPage = IndexPage - 1;
                }
                if (dbResponse.Count() > 0) {
                    dbResponse = await familyDam.GetListOffset(dbResponse, IndexPage, tableState.PageRows, Takep);
                }

                var response = new List<ApplicationFamily>();
                foreach (var dbTask in dbResponse) {
                    response.Add(new ApplicationFamily {
                        Id = dbTask.Id,
                        Family = dbTask.Family
                    });
                }
                return response;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
                return null;
            }

        }
    }
}
