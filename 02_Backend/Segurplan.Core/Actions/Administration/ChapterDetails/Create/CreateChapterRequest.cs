using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.Create {
    public class CreateChapterRequest : IRequest<IRequestResponse<CreateChapterResponse>> {
        public string Title { get; set; }
    }
}
