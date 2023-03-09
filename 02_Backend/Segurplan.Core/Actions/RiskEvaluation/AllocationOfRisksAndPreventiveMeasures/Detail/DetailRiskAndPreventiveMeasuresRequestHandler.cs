using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Detail {
    public class DetailRiskAndPreventiveMeasuresRequestHandler : IRequestHandler<DetailRiskAndPreventiveMeasuresRequest, IRequestResponse<DetailRiskAndPreventiveMeasuresResponse>> {

        private readonly SegurplanContext context;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;

        public DetailRiskAndPreventiveMeasuresRequestHandler(SegurplanContext context, UserManager<User> userManager, IMapper mapper) {
            this.context = context;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<DetailRiskAndPreventiveMeasuresResponse>> Handle(DetailRiskAndPreventiveMeasuresRequest request, CancellationToken cancellationToken) {

            DetailRiskAndPreventiveMeasuresResponse riskAndPrevMeasure = await context.RisksAndPreventiveMeasures
                                           .ProjectTo<DetailRiskAndPreventiveMeasuresResponse>(mapper.ConfigurationProvider)
                                           .FirstOrDefaultAsync(r => r.Id == request.Id);

            if (riskAndPrevMeasure != null) {
                riskAndPrevMeasure.CreatedByName = await GetUserName(riskAndPrevMeasure.CreatedBy);
                riskAndPrevMeasure.ModifiedByName = await GetUserName(riskAndPrevMeasure.ModifiedBy);
            }

            if (riskAndPrevMeasure is null)
                return RequestResponse.NotFound<DetailRiskAndPreventiveMeasuresResponse>();

            return RequestResponse.Ok(riskAndPrevMeasure);
        }

        private async Task<string> GetUserName(int userId) {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user != null)
                return user.UserName;

            return string.Empty;
        }
    }
}
