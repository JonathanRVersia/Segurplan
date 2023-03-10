﻿using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Segurplan.Core.Actions.Plans.PlanManagement.Generate.Plan;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.Web.Pages.Models.SafetyPlans {
    public class MyPlans : PlansBase {
        public MyPlans(IMediator mediator, ILogger<MyPlans> logger, UserManager<User> userManager)
            : base(mediator, logger, userManager) {
            PrevPage = "/MyPlans";
        }
    }
}
