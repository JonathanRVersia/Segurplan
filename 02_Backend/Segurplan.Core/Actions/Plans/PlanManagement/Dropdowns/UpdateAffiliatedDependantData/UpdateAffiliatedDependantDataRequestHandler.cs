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

namespace Segurplan.Core.Actions.Plans.PlanManagement.Dropdowns.UpdateAffiliatedDependantData {
    public class UpdateAffiliatedDependantDataRequestHandler : IRequestHandler<UpdateAffiliatedDependantDataRequest, IRequestResponse<UpdateAffiliatedDependantDataResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public UpdateAffiliatedDependantDataRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<UpdateAffiliatedDependantDataResponse>> Handle(UpdateAffiliatedDependantDataRequest request, CancellationToken cancellationToken) {

            return RequestResponse.Ok(new UpdateAffiliatedDependantDataResponse {

                FilteredDelegations = await context.Delegation
                                         .Where(x => x.BusinessAddressId == request.BusinessAddressId)
                                         .ProjectTo<SelectDataDto>(mapper.ConfigurationProvider)
                                         .ToListAsync()
            });

        }



    }
}
