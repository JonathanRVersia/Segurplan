using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Segurplan.Core.Actions.Administration.Risks;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.DataAccessManagers;

namespace Segurplan.Core.BusinessManagers {
    public class RiskListManager {
        public const string OrderById = "Id";
        public const string OrderByName = "Name";
        public const string OrderByCode = "Code";
        public const string FilterById = "Id";
        public const string FilterByName = "Name";
        public const string FilterByCode = "Code";
        public bool OrderModeDesc = false;

        public List<ApplicationRisk> Response { get; set; } = new List<ApplicationRisk>();
        public RiskListTableState TableState { get; set; }
        public RiskFilter TableFilter { get; set; }
        public int FilteredRisk { get; internal set; }
        public int IndexPage { get; internal set; }
        public int RemainingRows { get; internal set; }
        public int Takep { get; internal set; }

        private readonly RiskDam riskDam;

        public RiskListManager(RiskDam  riskDam) {
            this.riskDam = riskDam;
        }

        public async Task<List<ApplicationRisk>> GetRiskList(RiskListTableState tableState, RiskFilter tableFilter) {
            try {
                TableState = tableState;
                TableFilter = tableFilter;

                var dbResponse = await riskDam.SelectAll();

                if (TableFilter.Id != 0) {
                    dbResponse = await riskDam.ApplyFilters(dbResponse, FilterById, TableFilter.Id.ToString());
                }
                dbResponse = string.IsNullOrEmpty(TableFilter.Code) ? dbResponse : await riskDam.ApplyFilters(dbResponse, FilterByCode, TableFilter.Code);
                dbResponse = string.IsNullOrEmpty(TableFilter.Name)? dbResponse : await riskDam.ApplyFilters(dbResponse, FilterByName, TableFilter.Name);
                //After filtering we need to count results
                FilteredRisk = dbResponse.Count();
                IndexPage = tableState.IndexPage;
                if (tableState.OrderModeDesc == Helpers.ListOrderMode.Desc) { OrderModeDesc = true; }
                dbResponse = await riskDam.OrderList(dbResponse, tableState.OrderBy, OrderModeDesc);

                //check if enought rows remaining to fill the response
                RemainingRows = FilteredRisk - (tableState.PageRows * IndexPage);
                Takep = tableState.PageRows;
                if (RemainingRows - tableState.PageRows < 0) {
                    Takep = RemainingRows;
                }
                if (RemainingRows == 0) {
                    //not enough rows to fill this page, lets see if enough row to fill indexPage - 1 because we will check planlist.count() later
                    IndexPage = IndexPage - 1;
                }
                if (dbResponse.Count() > 0) {
                    dbResponse = await riskDam.GetListOffset(dbResponse, IndexPage, tableState.PageRows, Takep);
                }

                var response = new List<ApplicationRisk>();
                foreach (var dbRisk in dbResponse) {
                    response.Add(new ApplicationRisk {
                        Id = dbRisk.Id,
                        Name = dbRisk.Name,
                        Code = dbRisk.Code
                    }) ;
                }
                return response;

            } catch (Exception e) {
                Debug.WriteLine(e.ToString());
                return null;
            }
        }
    }
}
