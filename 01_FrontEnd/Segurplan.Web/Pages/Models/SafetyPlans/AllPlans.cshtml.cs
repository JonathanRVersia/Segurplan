using System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.Web.Pages.Models.SafetyPlans {
    [ValidateAntiForgeryToken]
    public class AllPlans : PlansBase {
        public AllPlans(IMediator mediator, ILogger<MyPlans> logger, UserManager<User> userManager)
            : base(mediator, logger, userManager) {
            PrevPage = "/AllPlans";
        }
    }
}
