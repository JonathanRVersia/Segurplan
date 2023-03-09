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
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Read.View {
    public class ViewPlanPlanesRequestHandler : IRequestHandler<ViewPlanPlanesRequest, IRequestResponse<ViewPlanPlanesResponse>> {

        private readonly SegurplanContext dbContext;
        private readonly IMapper mapper;

        public ViewPlanPlanesRequestHandler(SegurplanContext dbContext, IMapper mapper) {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<ViewPlanPlanesResponse>> Handle(ViewPlanPlanesRequest request, CancellationToken cancellationToken) {

            List<ApplicationPlaneFamily> planeList = new List<ApplicationPlaneFamily>();

            if (!request.OnlySelected) {
                planeList = await dbContext.PlaneFamily
                               .Where(pl => pl.Plane.Any())
                               .ProjectTo<ApplicationPlaneFamily>(mapper.ConfigurationProvider)
                               .ToListAsync();
            }

            List<SafetyPlanPlane> planPlaneList = await dbContext.SafetyStudyPlanPlane
                .Where(pp => pp.IdSafetyStudyPlan == request.PlanId)
                .ProjectTo<SafetyPlanPlane>(mapper.ConfigurationProvider).ToListAsync();

            return !planeList.Any() && !planPlaneList.Any() ? RequestResponse.NotFound(new ViewPlanPlanesResponse()) : RequestResponse.Ok(new ViewPlanPlanesResponse {
                PlaneList = planeList,
                PlanPlaneList = planPlaneList
            });
        }
    }
}
