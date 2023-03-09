using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.Core.Actions.Administration.ChapterDetails.DeleteCheck;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.DeleteChapterVersionCheck {
    public class DeleteChapterVersionCheckRequest : IRequest<IRequestResponse<DeleteCheckChapterResponse>> {
        public int ChapterVersionId { get; set; }
    }
}
