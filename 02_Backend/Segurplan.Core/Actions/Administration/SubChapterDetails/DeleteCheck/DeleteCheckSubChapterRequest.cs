using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.SubChapterDetails.DeleteCheck {
    public class DeleteCheckSubChapterRequest : IRequest<IRequestResponse<DeleteCheckSubChapterResponse>> {
        public int SubChapterId { get; set; }

        public List<int> SubChapterIds { get; set; }

        public int ChapterId { get; set; }
    }
}
