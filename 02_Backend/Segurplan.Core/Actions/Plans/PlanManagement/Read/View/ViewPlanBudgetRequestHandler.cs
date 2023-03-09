using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.BusinessObjects;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Read.View {
    public class ViewPlanBudgetRequestHandler : IRequestHandler<ViewPlanBudgetRequest, IRequestResponse<ViewPlanBudgetResponse>> {

        private readonly SegurplanContext dbContext;
        private readonly IMapper mapper;
        public List<ApplicationTask> TaskArticles { get; set; }
        public List<ApplicationArticle> SelectedArticlesDB { get; set; }

        public ViewPlanBudgetRequestHandler(SegurplanContext dbContext, IMapper mapper) {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<ViewPlanBudgetResponse>> Handle(ViewPlanBudgetRequest request, CancellationToken cancellationToken) {

            List<ApplicationArticleFamily> articlesByFamily = new List<ApplicationArticleFamily>();
            List<ApplicationTask> articlesByTask = new List<ApplicationTask>();
            List<BudgetDetail> budgetDetailsList = new List<BudgetDetail>();
            Budget budget = new Budget();

            if (!request.OnlySelected) {
                articlesByFamily = await dbContext.ArticleFamily
                               .Where(pl => pl.Article.Any())
                               .ProjectTo<ApplicationArticleFamily>(mapper.ConfigurationProvider)
                               .ToListAsync();


                articlesByTask = await dbContext.Tasks
                           .Where(pl => pl.ArticleTaskDetails.Any())
                           .ProjectTo<ApplicationTask>(mapper.ConfigurationProvider)
                           .ToListAsync();

                budgetDetailsList = await dbContext.BudgetDetail
                           .Where(pl => pl.IdBudget == request.BudgetId)
                           .ToListAsync();

                budget = await dbContext.Budget
                           .Where(pl => pl.Id == request.BudgetId)
                           .FirstOrDefaultAsync();
            }

            var result = articlesByTask.Count;

            SetDefaultValues(articlesByTask, request, budgetDetailsList);

            if (!articlesByFamily.Any() && articlesByTask.Any()) {
                return RequestResponse.NotFound(new ViewPlanBudgetResponse());
            } else {
                return RequestResponse.Ok(new ViewPlanBudgetResponse {
                    ArticlesFamily = articlesByFamily,
                    ArticlesByTask = TaskArticles,
                    SelectedArticlesDB = SelectedArticlesDB,
                    ApplicablePercentage = budget != null ? budget.ApplicabePercentage : 100
                });
            }
        }

        private void SetDefaultValues(List<ApplicationTask> articlesByTask, ViewPlanBudgetRequest request, List<BudgetDetail> budgetDetailsList) {
            List<ApplicationArticle> articles;
            TaskArticles = new List<ApplicationTask>();
            SelectedArticlesDB = new List<ApplicationArticle>();
            foreach (var task in articlesByTask) {
                articles = new List<ApplicationArticle>();
                foreach (var article in task.TaskDetails) {

                    var articleWithValues = GetArticleWithCalculationValues(article.Article, request);
                    articles.Add(articleWithValues);
                    var articleFromDB = new BudgetDetail();
                    if (budgetDetailsList != null) {
                        articleFromDB = budgetDetailsList.Find(x => x.IdArticle == article.Article.Id);
                    }
                    if (articleFromDB != null) {
                        var articleDB = new ApplicationArticle();

                        articleDB.IdArticleFamily = articleWithValues.IdArticleFamily;
                        articleDB.Name = articleWithValues.Name;
                        articleDB.Percentage = articleWithValues.Percentage;
                        articleDB.Price = articleWithValues.Price;
                        articleDB.PriceDurationWork = articleFromDB.UnitPrice;
                        articleDB.TimeOfWork = articleWithValues.TimeOfWork;
                        articleDB.MinimumUnit = articleWithValues.MinimumUnit;
                        articleDB.Id = articleWithValues.Id;
                        articleDB.AmortizationPrice = articleWithValues.AmortizationPrice;
                        articleDB.AmortizationTime = articleWithValues.AmortizationTime;

                        articleDB.Unit = articleFromDB.QuantityUnits;
                        articleDB.TotalPrice = articleFromDB.UnitPrice * articleFromDB.QuantityUnits;
                        var exist = SelectedArticlesDB.Find(x => x.Id == articleDB.Id);
                        if (exist == null) {
                            SelectedArticlesDB.Add(articleDB);
                        }
                    }
                }

                TaskArticles.Add(new ApplicationTask() {
                    Id = task.Id,
                    Name = task.Name,
                    Articles = articles
                });
            }
        }

        private ApplicationArticle GetArticleWithCalculationValues(ApplicationArticle article, ViewPlanBudgetRequest request) {
            var isByTimeOfWork = false;
            isByTimeOfWork = article.AmortizationTime == 0 && article.TimeOfWork != 0 ? true : false;
            //Precio Amortización dia
            if (isByTimeOfWork) {
                article.AmortizationPrice = article.Price * article.TimeOfWork;
            } else if (article.AmortizationTime != 0) {
                article.AmortizationPrice = article.Price / (365 * article.AmortizationTime);
            } else {
                article.AmortizationPrice = 0;
            }

            //Precio duración de la obra
            article.PriceDurationWork = !isByTimeOfWork ? Math.Round((article.AmortizationPrice * request.DurationWork), 2) : article.AmortizationPrice;

            //Se asigna el Price como precio duración de la obra
            if (article.AmortizationTime == 0 && article.TimeOfWork == 0) 
                article.PriceDurationWork = Math.Round(article.Price, 2);

            int units;
            //Cantidad por pocentaje
            if (article.Percentage != 0) {
                units = GetUnitsByPercentage(request.NumberWorkers, article.Percentage);
                article.Unit = units >= article.MinimumUnit ? units : article.MinimumUnit;
            } else//Cantidad funcion de la obra
                if (article.TimeOfWork != 0) {
                units = GetUnitsByTimeOfWork(article.TimeOfWork, request.DurationWork);
                article.Unit = units >= article.MinimumUnit ? units : article.MinimumUnit;
            } else {
                article.Unit = article.MinimumUnit;
            }

            article.TotalPrice = article.Unit * article.PriceDurationWork;

            return article;
        }

        private int GetUnitsByPercentage(int numberWorkers, int percentage) {
            decimal result = (decimal)(numberWorkers * percentage) / 100;
            return (int)Math.Ceiling(result);
        }

        private int GetUnitsByTimeOfWork(decimal timeOfWork, int durationWork) {
            decimal result = (decimal)timeOfWork * durationWork;
            return (int)Math.Ceiling(result);
        }
    }
}

