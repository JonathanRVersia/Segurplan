using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Segurplan.Core.Actions.Administration;
using Segurplan.Core.Actions.Administration.Articles.Delete;
using Segurplan.Core.Actions.Administration.Articles.Detail;
using Segurplan.Core.Actions.Administration.Articles.FamilyDataList;
using Segurplan.Core.Actions.Administration.Articles.Save;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Web.Pages.Models.Administration.Articles {
    public class ArticleManagementModel : PageModel {
        public IMediator mediator;
        public ILogger<ArticleManagementModel> logger;
        public UserManager<User> userManager;

        public int CurrentUserId { get; private set; }
        public bool EditMode { get; set; }

        [BindProperty]
        public ApplicationArticle Article { get; set; } = new ApplicationArticle();
        public List<ApplicationArticleFamily> Family { get; set; }

        [BindProperty]
        public AdministrationActionType CurrentOperation { get; set; }

        public ArticleManagementModel(IMediator mediator, ILogger<ArticleManagementModel> logger, UserManager<User> userManager) {
            this.mediator = mediator;
            this.logger = logger;
            this.userManager = userManager;
        }
        public async Task<IActionResult> OnGetManageArticle(AdministrationActionType currentOperation, int articleId) {
            CurrentOperation = currentOperation;
            
            return await GetOrCreateArticle(articleId).ConfigureAwait(true);
        }
        public async Task<IActionResult> GetOrCreateArticle(int articleId) {
            if (articleId == 0) {
                CurrentOperation = AdministrationActionType.Create;
                Article.CreateDate = DateTime.Now;
                Article.UpdateDate = DateTime.Now;
                return Page();
            }

            var response = await mediator.Send(new ArticleDetailRequest { Id = articleId }).ConfigureAwait(true);

            if (response.Status != RequestStatus.Ok)
                return new LocalRedirectResult("/Error");

            Article = response.Value.Article;
            Family = response.Value.Family;

            return Page();
        }
        public async Task<IActionResult> OnPostSaveArticle() {
            CurrentUserId = Convert.ToInt32(userManager.GetUserId(HttpContext.User));

            var response = await mediator.Send(new SaveArticleRequest {
                Article = Article,
                UserId = CurrentUserId
            }).ConfigureAwait(true);
            if (response.Status == RequestStatus.Ok) {
                return LocalRedirect("/ArticlesList");
            } else {
                return new LocalRedirectResult("/Error");
            }
        }

        public async Task<IActionResult> OnGetCreateArticle() {
            CurrentOperation = AdministrationActionType.Create;
            CurrentUserId = Convert.ToInt32(userManager.GetUserId(HttpContext.User));
            var response  = await mediator.Send(new FamilyDataListRequest()).ConfigureAwait(true);

            if (response.Status != RequestStatus.Ok)
                return new LocalRedirectResult("/Error");

            Family = response.Value.Family;
            return await GetOrCreateArticle(default).ConfigureAwait(true);
        }
        public async Task<IActionResult> OnPostDeleteArticleAsync(int deleteArticleId) {
            var response = await mediator.Send(new DeleteArticleRequest { Id = deleteArticleId }).ConfigureAwait(true);

            if (response.Status == RequestStatus.Ok) {
                return new LocalRedirectResult("/ArticlesList");
            } else {
                return new LocalRedirectResult($"/ArticlesList?handler=OnGetAsync&deleteErrors={true}");
            }
        }
    }
}
