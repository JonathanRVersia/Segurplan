using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Segurplan.Core.BusinessObjects;

namespace Segurplan.Web.Pages.Models.Validators {

    /* This validator is for client side validation only!
    * Validators that work with client side validation:
    * NotNull/NotEmpty
    * Matches(regex)
    * InclusiveBetween(range)
    * CreditCard
    * Email
    * EqualTo(cross-property equality comparison)
    * MaxLength
    * MinLength
    * Length
    */
    public class SafetyPlanGeneralDataModelValidator : AbstractValidator<SafetyPlanGeneralData> {

        //  comented validators not working, validation on SafetyPlanGeneralData
        //public SafetyPlanGeneralDataModelValidator() {

        //    //RuleFor(sPlanGeneralData => sPlanGeneralData.PlanTitle).NotEmpty();

        //    RuleFor(sPlanGeneralData => sPlanGeneralData.IdBusinessAddress).NotEmpty();

        //    //RuleFor(sPlanGeneralData => sPlanGeneralData.CenterName).NotEmpty();

        //    RuleFor(sPlanGeneralData => sPlanGeneralData.IdDelegation).NotEmpty();

        //    RuleFor(sPlanGeneralData => sPlanGeneralData.IdCustomer).NotEmpty();

        //    //RuleFor(sPlanGeneralData => sPlanGeneralData.CustomerDescription).NotEmpty();

        //    RuleFor(sPlanGeneralData => sPlanGeneralData.IdGeneralActivity).NotEmpty();

        //    RuleFor(sPlanGeneralData => sPlanGeneralData.AffiliatedId).NotEmpty();

        //    //RuleFor(sPlanGeneralData => sPlanGeneralData.AffiliatedName).NotEmpty();

        //    RuleFor(sPlanGeneralData => sPlanGeneralData.IdReviewer).NotEmpty();

        //    //RuleFor(sPlanGeneralData => sPlanGeneralData.CreatorName).NotEmpty();

        //    RuleFor(sPlanGeneralData => sPlanGeneralData.CreatorCategoryId).NotEmpty();
        //}
    }
}
