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
using Segurplan.Core.BusinessObjects;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Risks.Save {
    public class SaveRiskRequestHandler : IRequestHandler<SaveRiskRequest, IRequestResponse<SaveRiskResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public SaveRiskRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<IRequestResponse<SaveRiskResponse>> Handle(SaveRiskRequest request, CancellationToken cancellationToken) {

            bool correctSave = false;

            if (request.risk.Id != default) {
                correctSave = await EditBehaviourAsync(request) > 0;
            } else {
                correctSave = await CreateBehaviourAsync(request) > 0;
            }

            if (!correctSave)
                return RequestResponse.Error<SaveRiskResponse>();

            return RequestResponse.Ok(new SaveRiskResponse());
            
        }

        private async Task<int> CreateBehaviourAsync(SaveRiskRequest request) {
            var riskPrevio =  await context.Risk
                .ProjectTo<ApplicationRisk>(mapper.ConfigurationProvider)
                .OrderBy(x=>x.Code)
                .LastAsync();
            
            var risk = mapper.Map<DataAccessLayer.Database.DataTransferObjects.Risk>(request.risk);
            risk.Code = riskPrevio.Code+1;
            context.Risk.Add(risk);
            return await context.SaveChangesAsync();
        }

        private async Task<int> EditBehaviourAsync(SaveRiskRequest request) {
            var risk = context.Risk.FirstOrDefault(r=>r.Id==request.risk.Id);

            if (risk is null) return default;

            risk = mapper.Map<DataAccessLayer.Database.DataTransferObjects.Risk>(request.risk);

            context.Risk.Update(risk);
            return await context.SaveChangesAsync();
        }
    }
}
