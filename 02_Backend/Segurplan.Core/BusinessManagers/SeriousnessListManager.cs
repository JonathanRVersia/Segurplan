using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Segurplan.Core.Actions.Administration.Seriousness;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.DataAccessManagers;
using Segurplan.Core.Helpers;

namespace Segurplan.Core.BusinessManagers {

    public class SeriousnessListManager {
        public const string OrderById = "Id";
        public const string OrderByValue = "Value";
        public const string FilterById = "Id";
        public const string FilterByValue = "Value";
        public bool OrderModeDesc = false;

        public List<ApplicationSeriousness> Response { get; set; } = new List<ApplicationSeriousness>();
        public SeriousnessListTableState TableState { get; set; }
        public SeriousnessFilter TableFilter { get; set; }
        public int FilteredSeriousness { get; internal set; }
        public int IndexPage { get; internal set; }
        public int RemainingRows { get; internal set; }
        public int Takep { get; internal set; }

        private readonly SeriousnessDam seriousnessDam;

        public SeriousnessListManager(SeriousnessDam seriousnessDam) {
            this.seriousnessDam = seriousnessDam;
        }

        public async Task<List<ApplicationSeriousness>> GetSeriousnessList(SeriousnessListTableState tableState, SeriousnessFilter tableFilter) {
            try {
                TableState = tableState;
                TableFilter = tableFilter;

                var dbResponse = await seriousnessDam.SelectAll();

                if (TableFilter.Id != 0) {
                    dbResponse = await seriousnessDam.ApplyFilters(dbResponse, FilterById, TableFilter.Id.ToString());
                }
                
                dbResponse = string.IsNullOrEmpty(TableFilter.Value) ? dbResponse : await seriousnessDam.ApplyFilters(dbResponse, FilterByValue, TableFilter.Value);
                //After filtering we need to count results
                FilteredSeriousness = dbResponse.Count();                
                IndexPage = tableState.IndexPage;
                if (tableState.OrderModeDesc == Helpers.ListOrderMode.Desc) { OrderModeDesc = true; }
                dbResponse = await seriousnessDam.OrderList(dbResponse, tableState.OrderBy, OrderModeDesc);
                
                //check if enought rows remaining to fill the response
                RemainingRows = FilteredSeriousness - (tableState.PageRows * IndexPage);
                Takep = tableState.PageRows;
                if (RemainingRows - tableState.PageRows < 0) {
                    Takep = RemainingRows;
                }
                if (RemainingRows == 0) {
                    //not enough rows to fill this page, lets see if enough row to fill indexPage - 1 because we will check planlist.count() later
                    IndexPage = IndexPage - 1;
                }
                if (dbResponse.Count() > 0) {
                    dbResponse = await seriousnessDam.GetListOffset(dbResponse, IndexPage, tableState.PageRows, Takep);
                }
                                
                var response = new List<ApplicationSeriousness>();
                foreach (var dbSeriousness in dbResponse) {
                    response.Add(new ApplicationSeriousness {
                        Id = dbSeriousness.Id,
                        Value = dbSeriousness.Value
                    });
                }
                return response;

            } catch (Exception e) {
                Debug.WriteLine(e.ToString());
                return null;
            }
        }
    }
}

