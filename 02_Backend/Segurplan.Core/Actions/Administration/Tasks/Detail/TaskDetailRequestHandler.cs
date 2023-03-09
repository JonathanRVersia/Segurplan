using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.BusinessObjects;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Tasks.Detail {
    public class TaskDetailRequestHandler : IRequestHandler<TaskDetailRequest, IRequestResponse<TaskDetailResponse>> {
        
        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public TaskDetailRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<TaskDetailResponse>> Handle(TaskDetailRequest request, CancellationToken cancellationToken) {
            var result = await context.Tasks
                .ProjectTo<ApplicationTask>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(task => task.Id == request.Id);

            if (result is null) return RequestResponse.NotFound<TaskDetailResponse>();

            return RequestResponse.Ok(new TaskDetailResponse(result));
        }
    }
}
