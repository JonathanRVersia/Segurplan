using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Risks.Delete {
    public class DeleteRiskRequestHandler : IRequestHandler<DeleteRiskRequest,IRequestResponse<DeleteRiskResponse>> {
        
        protected readonly SegurplanContext context;

        public DeleteRiskRequestHandler(SegurplanContext context) {
            this.context = context;
        }

        public async Task<IRequestResponse<DeleteRiskResponse>> Handle(DeleteRiskRequest request, CancellationToken cancellationToken) {
            try 
            {
                context.Risk.Remove(new DataAccessLayer.Database.DataTransferObjects.Risk { Id=request.Id });
                int deleted = await context.SaveChangesAsync();
                return RequestResponse.Ok(new DeleteRiskResponse());

            } 
            catch(Exception e)
            {
                return RequestResponse.NotOk(new DeleteRiskResponse());
            }
        }
    }
}
