using System.Collections.Generic;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ChapterActivityList.Export {
    public class ChapterArtivityToWordRequest : IRequest<IRequestResponse<ChapterArtivityToWordResponse>> {
        public List<ChapterActivityListResponse.ListItem> Chapters { get; set; }
    }
}

