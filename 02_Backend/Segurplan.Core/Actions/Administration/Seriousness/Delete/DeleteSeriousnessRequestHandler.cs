using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Seriousness.Delete {
    public class DeleteSeriousnessRequestHandler : IRequestHandler<DeleteSeriousnessRequest, IRequestResponse<DeleteSeriousnessResponse>> {

        protected readonly SegurplanContext context;

        public DeleteSeriousnessRequestHandler(SegurplanContext context) {
            this.context = context;
        }

        public async Task<IRequestResponse<DeleteSeriousnessResponse>> Handle(DeleteSeriousnessRequest request, CancellationToken cancellationToken) {

            var existeRiesgoAsociado = context.RisksAndPreventiveMeasures.Where(x => x.SeriousnessId == request.Id).Count() != 0;

            if (existeRiesgoAsociado) {
                return RequestResponse.NotOk(new DeleteSeriousnessResponse());
            }

            context.Seriousness.Remove(new DataAccessLayer.Database.DataTransferObjects.Seriousness { Id = request.Id });
            int deleted = await context.SaveChangesAsync();

            if (deleted is 0) {
                return RequestResponse.NotOk(new DeleteSeriousnessResponse());
            }

            return RequestResponse.Ok(new DeleteSeriousnessResponse());
        }
    }
}
