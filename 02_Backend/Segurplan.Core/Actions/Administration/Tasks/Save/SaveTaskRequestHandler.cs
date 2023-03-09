using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Tasks.Save {
    public class SaveTaskRequestHandler : IRequestHandler<SaveTaskRequest, IRequestResponse<SaveTaskResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public SaveTaskRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<SaveTaskResponse>> Handle(SaveTaskRequest request, CancellationToken cancellationToken) {
            bool correctSave = true;

            var strategy = context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () => {

                using (var trans = context.Database.BeginTransaction()) {

                    try {

                        if (request.Task.Id != default) {
                            correctSave = await EditBehaviourAsync(request);
                        } else {
                            var newId = await CreateBehaviourAsync(request);
                            request.Task.Id = newId;
                        }
                        if (correctSave)
                            correctSave = await SaveDetailsTask(request);
                        else
                            trans.Rollback();

                        context.SaveChanges();

                        trans.Commit();
                    } catch (Exception e) {
                        trans.Rollback();
                    }
                }
            });

            if (!correctSave)
                return RequestResponse.Error<SaveTaskResponse>();

            return RequestResponse.Ok(new SaveTaskResponse());
        }
        private async Task<bool> SaveDetailsTask(SaveTaskRequest request) {
            List<ArticleTaskDetail> details = new List<ArticleTaskDetail>();
            var current = context.ArticleTaskDetail.Where(x => x.IdTasks == request.Task.Id).ToList();

            if (current.Count > 0) {
                context.ArticleTaskDetail.RemoveRange(current);
            }

            if (request.Task.TaskDetails != null) {
                foreach (var item in request.Task.TaskDetails) {

                    details.Add(new ArticleTaskDetail {
                        IdTasks = request.Task.Id,
                        IdArticle = item.Article.Id,
                        CreateDate = item.CreateDate != new DateTime() ? item.CreateDate : DateTime.UtcNow ,
                        CreatedBy = item.CreatedBy != 0 ? item.CreatedBy : request.UserId,
                        ModifiedBy = request.UserId,
                        UpdateDate = DateTime.UtcNow,
                    });
                }
                await context.ArticleTaskDetail.AddRangeAsync(details);
            }
            return true;
        }
        private async Task<int> CreateBehaviourAsync(SaveTaskRequest request) {
            var task = mapper.Map<Segurplan.DataAccessLayer.Database.DataTransferObjects.Tasks>(request.Task);
            task.CreatedBy = request.UserId;
            task.ModifiedBy = request.UserId;
            await context.Tasks.AddAsync(task);
            await context.SaveChangesAsync();
            return task.Id;
        }
        private async Task<bool> EditBehaviourAsync(SaveTaskRequest request) {
            var task = context.Tasks.FirstOrDefault(r => r.Id == request.Task.Id);

            if (task is null) return default;

            task = mapper.Map<Segurplan.DataAccessLayer.Database.DataTransferObjects.Tasks>(request.Task);
            MapCustomTask(task, request);

            context.Tasks.Update(task);
            return true;
        }
        private void MapCustomTask(Segurplan.DataAccessLayer.Database.DataTransferObjects.Tasks task, SaveTaskRequest request) {
            task.UpdateDate = DateTime.Now;
            task.ModifiedBy = request.UserId;
        }
    }
}
