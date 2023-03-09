using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Tasks.Delete {
    public class DeleteTaskRequestHandler : IRequestHandler<DeleteTaskRequest, IRequestResponse<DeleteTaskResponse>> {
        protected readonly SegurplanContext context;
        public DeleteTaskRequestHandler(SegurplanContext context) {
            this.context = context;
        }
        public async Task<IRequestResponse<DeleteTaskResponse>> Handle(DeleteTaskRequest request, CancellationToken cancellationToken) {
            try {
                var strategy = context.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () => {

                    using (var trans = context.Database.BeginTransaction()) {

                        try {
                            var currentDetails = context.ArticleTaskDetail.Where(x => x.IdTasks == request.Id).ToList();
                            
                            context.ArticleTaskDetail.RemoveRange(currentDetails);

                            context.Tasks.Remove(new DataAccessLayer.Database.DataTransferObjects.Tasks { Id = request.Id });

                            await context.SaveChangesAsync();

                            trans.Commit();

                        } catch (Exception e) {
                            trans.Rollback();
                        }
                    }
                });
                return RequestResponse.Ok(new DeleteTaskResponse());

            } catch (Exception e) {
                return RequestResponse.NotOk(new DeleteTaskResponse());
            }
        }
    }
}
