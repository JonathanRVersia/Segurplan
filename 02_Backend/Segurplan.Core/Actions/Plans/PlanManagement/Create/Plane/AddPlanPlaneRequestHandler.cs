using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Segurplan.Core.BusinessObjects;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Create.Plane {
    public class AddPlanPlaneRequestHandler : IRequestHandler<AddPlanPlaneRequest, IRequestResponse<AddPlanPlaneResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public AddPlanPlaneRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<AddPlanPlaneResponse>> Handle(AddPlanPlaneRequest request, CancellationToken cancellationToken) {

            SafetyStudyPlanPlane planPlane = mapper.Map<SafetyStudyPlanPlane>(request);

            if (planPlane.Files.Any()) {
                planPlane.ContainsFile = true;
            }

            context.Add(planPlane);
            int result = context.SaveChanges();

            SafetyPlanPlane safetyPlanPlane = mapper.Map<SafetyPlanPlane>(planPlane);

            return result > 0 ?
                 RequestResponse.Ok(new AddPlanPlaneResponse(safetyPlanPlane)) :
                 RequestResponse.NotFound<AddPlanPlaneResponse>();
        }
    }
}
