using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Seriousness.MatrixDataList {
    public class MatrixDataListRequestHandler : IRequestHandler<MatrixDataListRequest, IRequestResponse<MatrixDataListResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public MatrixDataListRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<MatrixDataListResponse>> Handle(MatrixDataListRequest request, CancellationToken cancellationToken) {

            var probability = await context.Probability.ProjectTo<MatrixProbabilityDTO>(mapper.ConfigurationProvider).ToListAsync();
            var riskLevel = await context.RiskLevel.ProjectTo<MatrixRiskLevelDTO>(mapper.ConfigurationProvider).ToListAsync();

            if (!probability.Any()) {
                return RequestResponse.NotFound<MatrixDataListResponse>();
            }

            return RequestResponse.Ok(new MatrixDataListResponse(probability, riskLevel));
        }
    }
}
