using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Read.View.PlanesDropdowns {
    public class ViewFamiliesPlanesDropdownsRequestHandler : IRequestHandler<ViewFamiliesPlanesDropdownsRequest, IRequestResponse<ViewFamiliesPlanesDropdownsResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public ViewFamiliesPlanesDropdownsRequestHandler(SegurplanContext context, IMapper mapper) {

            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<ViewFamiliesPlanesDropdownsResponse>> Handle(ViewFamiliesPlanesDropdownsRequest request, CancellationToken cancellationToken) {

            List<FamiliesPlanesDropdowns> result = await context.PlaneFamily
                .ProjectTo<FamiliesPlanesDropdowns>(mapper.ConfigurationProvider)
                .ToListAsync();


            return result.Any() ?
                RequestResponse.Ok(new ViewFamiliesPlanesDropdownsResponse(result)) :
                RequestResponse.NotFound<ViewFamiliesPlanesDropdownsResponse>();
        }
    }
}
