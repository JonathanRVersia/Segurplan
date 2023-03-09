using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;

namespace Segurplan.Core.Extensions {
    public class AjaxAttribute : ActionMethodSelectorAttribute {
        public string HttpVerb { get; set; }

        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action) {
            return routeContext.HttpContext.Request.IsAjax(HttpVerb);
        }
    }
}
