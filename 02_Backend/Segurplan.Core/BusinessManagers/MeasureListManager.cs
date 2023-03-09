using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Segurplan.Core.Actions.Administration.PreventiveMeasures;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.DataAccessManagers;
using Segurplan.Core.Helpers;

namespace Segurplan.Core.BusinessManagers {

    public class MeasureListManager {
        public const string OrderByCode = "Code";
        public const string OrderByMeasure = "Measure";
        public const string FilterByCode = "Code";
        public const string FilterByMeasure = "Measure";
        public bool OrderModeDesc = false;

        public List<ApplicationPreventiveMeasure> Response { get; set; } = new List<ApplicationPreventiveMeasure>();
        public MeasureListTableState TableState { get; set; }
        public PreventiveMeasuresFilter TableFilter { get; set; }
        public int FilteredMeasures { get; internal set; }
        public int IndexPage { get; internal set; }
        public int RemainingRows { get; internal set; }
        public int Takep { get; internal set; }

        private readonly PreventiveMeasureDam measureDam;


        public MeasureListManager(PreventiveMeasureDam measureDam) {
            this.measureDam = measureDam;
        }



        public async Task<List<ApplicationPreventiveMeasure>> GetPreventiveMeasuresList(MeasureListTableState tableState, PreventiveMeasuresFilter tableFilter) {
            try {
                TableState = tableState;
                TableFilter = tableFilter;

                var dbResponse = await measureDam.SelectAll();
                dbResponse = string.IsNullOrEmpty(TableFilter.Code) ? dbResponse : await measureDam.ApplyFilters(dbResponse, FilterByCode, TableFilter.Code);
                dbResponse = string.IsNullOrEmpty(TableFilter.Measure) ? dbResponse : await measureDam.ApplyFilters(dbResponse, FilterByMeasure, TableFilter.Measure);
                //After filtering we need to count results
                FilteredMeasures = dbResponse.Count();                
                IndexPage = tableState.IndexPage;
                if (tableState.OrderModeDesc == Helpers.ListOrderMode.Desc) { OrderModeDesc = true; }
                dbResponse = await measureDam.OrderList(dbResponse, tableState.OrderBy, OrderModeDesc);
                
                //check if enought rows remaining to fill the response
                RemainingRows = FilteredMeasures - (tableState.PageRows * IndexPage);
                Takep = tableState.PageRows;
                if (RemainingRows - tableState.PageRows < 0) {
                    Takep = RemainingRows;
                }
                if (RemainingRows == 0) {
                    //not enough rows to fill this page, lets see if enough row to fill indexPage - 1 because we will check planlist.count() later
                    IndexPage = IndexPage - 1;
                }
                if (dbResponse.Count() > 0) {
                    dbResponse = await measureDam.GetListOffset(dbResponse, IndexPage, tableState.PageRows, Takep);
                }
                                
                var response = new List<ApplicationPreventiveMeasure>();
                foreach (var dbMeasure in dbResponse) {

                    response.Add(new ApplicationPreventiveMeasure {
                        Id = dbMeasure.Id,
                        Code = dbMeasure.Code,
                        Desciption = dbMeasure.Description
                    });

                }
                //ApplicationPreventiveMeasure
                return response;

            } catch (Exception e) {
                Debug.WriteLine(e.ToString());
                return null;
            }

        }





    }
}

