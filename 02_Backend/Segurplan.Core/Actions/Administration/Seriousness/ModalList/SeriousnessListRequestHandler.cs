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

namespace Segurplan.Core.Actions.Administration.Seriousness.ModalList {
    public class SeriousnessListRequestHandler : IRequestHandler<SeriousnessListRequest, IRequestResponse<SeriousnessListResponse>> {
        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public SeriousnessListRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<SeriousnessListResponse>> Handle(SeriousnessListRequest request, CancellationToken cancellationToken) {
            var seriousness = await context.Seriousness
                                        .ProjectTo<SeriousnessListResponse.ListItem>(mapper.ConfigurationProvider)
                                        .RunSpecification(request.Specifications);

            return RequestResponse.Ok(new SeriousnessListResponse(seriousness));
        }
    }
}
