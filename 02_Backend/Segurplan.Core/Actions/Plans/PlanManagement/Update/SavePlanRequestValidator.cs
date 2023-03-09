using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Segurplan.Core.BusinessObjects;
using Segurplan.FrameworkExtensions.MediatR.Validation;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Update {

    public class SavePlanRequestValidator : RequestValidator<SavePlanRequest> {

        public SavePlanRequestValidator(IStringLocalizer localizer) {

            //RuleFor(request => request.PlanInformation.GeneralData.PlanTitle).NotEmpty().WithMessage(_ => localizer["Validation.Required"]);

            //RuleFor(request => request.PlanInformation.GeneralData.IdBusinessAddress).NotEmpty().WithMessage(_ => localizer["Validation.Required"]);

            ////RuleFor(request => request.PlanInformation.GeneralData.CenterName).NotEmpty().WithMessage(_ => localizer["Validation.Required"]);

            //RuleFor(request => request.PlanInformation.GeneralData.IdDelegation).NotEmpty().WithMessage(_ => localizer["Validation.Required"]);

            //RuleFor(request => request.PlanInformation.GeneralData.IdCustomer).NotEmpty().WithMessage(_ => localizer["Validation.Required"]);

            //RuleFor(request => request.PlanInformation.GeneralData.CustomerDescription).NotEmpty().WithMessage(_ => localizer["Validation.Required"]);

            //RuleFor(request => request.PlanInformation.GeneralData.IdGeneralActivity).NotEmpty().WithMessage(_ => localizer["Validation.Required"]);

            //RuleFor(request => request.PlanInformation.GeneralData.AffiliatedId).NotEmpty().WithMessage(_ => localizer["Validation.Required"]);

            //RuleFor(request => request.PlanInformation.GeneralData.AffiliatedName).NotEmpty().WithMessage(_ => localizer["Validation.Required"]);

            //RuleFor(request => request.PlanInformation.GeneralData.IdReviewer).NotEmpty().WithMessage(_ => localizer["Validation.Required"]);

            //RuleFor(request => request.PlanInformation.GeneralData.CreatorName).NotEmpty().WithMessage(_ => localizer["Validation.Required"]);

            //RuleFor(request => request.PlanInformation.GeneralData.CreatorCategoryId).NotEmpty().WithMessage(_ => localizer["Validation.Required"]);

        }
    }
}
