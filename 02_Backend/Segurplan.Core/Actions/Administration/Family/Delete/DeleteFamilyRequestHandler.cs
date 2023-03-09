using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Family.Delete {
    public class DeleteFamilyRequestHandler : IRequestHandler<DeleteFamilyRequest, IRequestResponse<DeleteFamilyResponse>> {

        protected readonly SegurplanContext context;

        public DeleteFamilyRequestHandler(SegurplanContext context) {
            this.context = context;
        }

        public async Task<IRequestResponse<DeleteFamilyResponse>> Handle(DeleteFamilyRequest request, CancellationToken cancellationToken) {
            try {
                context.ArticleFamily.Remove(new DataAccessLayer.Database.DataTransferObjects.ArticleFamily { Id = request.Id });
                int deleted = await context.SaveChangesAsync();
                return RequestResponse.Ok(new DeleteFamilyResponse());

            } catch (Exception e) {
                return RequestResponse.NotOk(new DeleteFamilyResponse());
            }
        }
    }
}
