using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.SubChapterDetails.DetailDropdowns {
    public class SubChapterDetailsDropdownsRequestHandler : IRequestHandler<SubChapterDetailsDropdownsRequest, IRequestResponse<SubChapterDetailsDropdownsResponse>> {
        private readonly IMapper mapper;
        private readonly SegurplanContext context;

        public SubChapterDetailsDropdownsRequestHandler(IMapper mapper, SegurplanContext context) {
            this.mapper = mapper;
            this.context = context;
        }

        public Task<IRequestResponse<SubChapterDetailsDropdownsResponse>> Handle(SubChapterDetailsDropdownsRequest request, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }
    }
}
