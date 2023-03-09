using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Segurplan.Core.Actions.Administration;
using Segurplan.Core.Actions.Administration.Articles.ModalList.Specifications;
using Segurplan.Core.Actions.Administration.Tasks.Delete;
using Segurplan.Core.Actions.Administration.Tasks.Detail;
using Segurplan.Core.Actions.Administration.Tasks.Save;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.FrameworkExtensions.MediatR;
using Segurplan.FrameworkExtensions.Query;

namespace Segurplan.Web.Pages.Models.Administration.TasksArticle {
    public class TaskManagementModel : PageModel
    {
        public IMediator mediator;
        public ILogger<TaskManagementModel> logger;
        public UserManager<User> userManager;

        public int CurrentUserId { get; private set; }
        [BindProperty]
        public ApplicationTask Task { get; set; } = new ApplicationTask();

        [BindProperty]
        public AdministrationActionType CurrentOperation { get; set; }
        public List<string> SelectedArticlesCodes { get; set; } = new List<string>();
        public int ArticleModalPageNumber { get; set; } = 1;
        public IRequestResponse<Segurplan.Core.Actions.Administration.Articles.ModalList.ArticlesListResponse> ArticlesList { get; set; }
        public int ArticlesModalPageSize = 10;
        private string ArticlesSearchContanisWord;

        public TaskManagementModel(IMediator mediator, ILogger<TaskManagementModel> logger, UserManager<User> userManager) {
            this.mediator = mediator;
            this.logger = logger;
            this.userManager = userManager;
        }
        public async Task<IActionResult> OnGetManageTask(AdministrationActionType currentOperation, int taskId) {
            CurrentOperation = currentOperation;

            return await GetOrCreateTask(taskId).ConfigureAwait(true);
        }
        public async Task<IActionResult> GetOrCreateTask(int taskId) {

            await GetArticlesList().ConfigureAwait(true);

            if (taskId == 0) {
                CurrentOperation = AdministrationActionType.Create;
                Task.CreateDate = DateTime.Now;
                Task.UpdateDate = DateTime.Now;
                Task.TaskDetails = new List<ApplicationArticleTaskDetail>();
                return Page();
            } else {

                var taskResponse = await mediator.Send(new TaskDetailRequest { Id = taskId }).ConfigureAwait(true);

                if (taskResponse.Status != RequestStatus.Ok)
                    return new LocalRedirectResult("/Error");

                Task = taskResponse.Value.Task;
                
                await FillSelectedCodes().ConfigureAwait(true);
            }

            return Page();
        }
        private async Task FillSelectedCodes() {
            if (Task.TaskDetails.Any()) {
                SelectedArticlesCodes = Task.TaskDetails.Select(x => x.Article.Id.ToString()).ToList();
            }
        }
        private async Task GetArticlesList() {
           ArticlesList  = await mediator.Send(new Segurplan.Core.Actions.Administration.Articles.ModalList.ArticlesListRequest() { Specifications = GetSpecifications() }).ConfigureAwait(true);
        }
        private IEnumerable<ISpecification<Segurplan.Core.Actions.Administration.Articles.ModalList.ArticlesListResponse.ListItem>> GetSpecifications() {
            var specifications = new List<ISpecification<Segurplan.Core.Actions.Administration.Articles.ModalList.ArticlesListResponse.ListItem>>();

            specifications.Add(GetPagination());
            specifications.Add(GetFilters());

            return specifications;
        }
        private ISpecification<Segurplan.Core.Actions.Administration.Articles.ModalList.ArticlesListResponse.ListItem> GetFilters() {
            ArticlesSpecification searchFilter = new ArticlesSpecification();

            if (!string.IsNullOrEmpty(ArticlesSearchContanisWord))
                searchFilter.ByContainsWord(ArticlesSearchContanisWord);

            return searchFilter;
        }
        private ISpecification<Segurplan.Core.Actions.Administration.Articles.ModalList.ArticlesListResponse.ListItem> GetPagination() {
            PaginationArticleSpecification defaultPagination = new PaginationArticleSpecification(ArticleModalPageNumber, ArticlesModalPageSize);

            if (ArticlesList == null)
                return defaultPagination;

            int currentPage = ArticlesList.Value.Page.Value;

            PaginationArticleSpecification paginated = new PaginationArticleSpecification(currentPage, currentPage + ArticlesModalPageSize);

            return paginated;
        }
        public async Task<IActionResult> OnPostSaveTask() {
            CurrentUserId = Convert.ToInt32(userManager.GetUserId(HttpContext.User));

            var response = await mediator.Send(new SaveTaskRequest {
                Task = Task,
                UserId = CurrentUserId
            }).ConfigureAwait(true);
            if (response.Status == RequestStatus.Ok) {
                return LocalRedirect("/TasksList");
            } else {
                return new LocalRedirectResult("/Error");
            }
        }

        public async Task<IActionResult> OnGetCreateTask() {
            CurrentOperation = AdministrationActionType.Create;
            CurrentUserId = Convert.ToInt32(userManager.GetUserId(HttpContext.User));

            return await GetOrCreateTask(default).ConfigureAwait(true);
        }
        public async Task<IActionResult> OnPostDeleteTaskAsync(int deleteTaskId) {
            var response = await mediator.Send(new DeleteTaskRequest { Id = deleteTaskId }).ConfigureAwait(true);

            if (response.Status == RequestStatus.Ok) {
                return new LocalRedirectResult("/TasksList");
            } else {
                return new LocalRedirectResult($"/TasksList?handler=OnGetAsync&deleteErrors={true}");
            }
        }
        public async Task<IActionResult> OnPostArticlesListPagination(string nextPage, string initialWord) {

            int page;

            bool success = Int32.TryParse(nextPage, out page);

            if (success) {
                ArticleModalPageNumber = page;
                ArticlesSearchContanisWord = initialWord;
                await GetArticlesList().ConfigureAwait(true);
            }

            if (ArticlesList.Status == RequestStatus.Ok) {
                return new JsonResult(ArticlesList.Value);
            } else {
                return StatusCode(404);
            }

        }
    }
}
