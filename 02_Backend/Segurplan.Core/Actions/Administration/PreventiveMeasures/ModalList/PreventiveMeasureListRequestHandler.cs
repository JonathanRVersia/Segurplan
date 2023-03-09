using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.PreventiveMeasures.ModalList {
    public class PreventiveMeasureListRequestHandler : IRequestHandler<PreventiveMeasureListRequest, IRequestResponse<PreventiveMeasureListResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public PreventiveMeasureListRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<PreventiveMeasureListResponse>> Handle(PreventiveMeasureListRequest request, CancellationToken cancellationToken) {

            var preventiveMeasures = await context.PreventiveMeasure
                                        .ProjectTo<PreventiveMeasureListResponse.ListItem>(mapper.ConfigurationProvider)
                                        .RunSpecification(request.Specifications);


            //if (!preventiveMeasures.Results.Any())
            //    return RequestResponse.NotFound<PreventiveMeasureListResponse>();

            return RequestResponse.Ok(new PreventiveMeasureListResponse(preventiveMeasures));
        }
    }
}
