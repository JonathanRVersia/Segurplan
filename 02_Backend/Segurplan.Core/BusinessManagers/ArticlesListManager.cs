using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Segurplan.Core.Actions.Administration.Articles;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.DataAccessManagers;

namespace Segurplan.Core.BusinessManagers {
    public class ArticlesListManager {
        public const string OrderById = "Id";
        public const string OrderByValue = "Value";
        public const string FilterByName = "Name";
        public const string FilterByFamily = "Family";
        public bool OrderModeDesc = false;

        public List<ApplicationArticle> Response { get; set; } = new List<ApplicationArticle>();
        public ArticlesListTableState TableState { get; set; }
        public ArticlesFilter TableFilter { get; set; }
        public int FilteredArticles { get; internal set; }
        public int IndexPage { get; internal set; }
        public int RemainingRows { get; internal set; }
        public int Takep { get; internal set; }

        private readonly ArticlesDam articlesDam;

        public ArticlesListManager(ArticlesDam articleDam) {
            this.articlesDam = articleDam;
        }

        public async Task<List<ApplicationArticle>> GetArticlesList(ArticlesListTableState tableState, ArticlesFilter tableFilter) {
            try {
                TableState = tableState;
                TableFilter = tableFilter;

                var dbResponse = await articlesDam.SelectAll();

                dbResponse = !string.IsNullOrEmpty(TableFilter.Name) ? await articlesDam.ApplyFilters(dbResponse, FilterByName, TableFilter.Name.ToString()) : dbResponse;

                dbResponse = !string.IsNullOrEmpty(TableFilter.Family) ? await articlesDam.ApplyFilters(dbResponse, FilterByFamily, TableFilter.Family) : dbResponse;


                FilteredArticles = dbResponse.Count();
                IndexPage = tableState.IndexPage;
                if (tableState.OrderModeDesc == Helpers.ListOrderMode.Desc) { OrderModeDesc = true; }
                dbResponse = await articlesDam.OrderList(dbResponse, tableState.OrderBy, OrderModeDesc);

                RemainingRows = FilteredArticles - (tableState.PageRows * IndexPage);
                Takep = tableState.PageRows;
                if (RemainingRows - tableState.PageRows < 0) {
                    Takep = RemainingRows;
                }
                if (RemainingRows == 0) {
                    IndexPage = IndexPage - 1;
                }
                if (dbResponse.Count() > 0) {
                    dbResponse = await articlesDam.GetListOffset(dbResponse, IndexPage, tableState.PageRows, Takep);
                }

                var response = new List<ApplicationArticle>();
                foreach (var dbArticles in dbResponse) {
                    response.Add(new ApplicationArticle {
                        Id = dbArticles.Id,
                        Name = dbArticles.Name,
                        IdArticleFamily = dbArticles.IdArticleFamily,
                        Percentage = dbArticles.Percentage,
                        TimeOfWork = dbArticles.TimeOfWork,
                        AmortizationTime = dbArticles.AmortizationTime,
                        MinimumUnit = dbArticles.MinimumUnit,
                        Price = dbArticles.Price,
                        FamilyName = dbArticles.IdArticleFamilyNavigation.Family
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
