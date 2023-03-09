using System.Linq;
using Segurplan.Core.Database.Models;
using SegurplanEnums = Segurplan.Core.Database.DataTransferObjects.Enum;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database.DataTransferObjects;
using Segurplan.Core.DTOs;
using System;

namespace Segurplan.Core.Database.DataAccessManagers {
    public class SafetyStudyPlanDAM : SegurplanDamBase<SafetyStudyPlanDTO>{

        public SafetyStudyPlanDAM(SegurplanContext sgpContext) : base(sgpContext){
            
        }

        public override SafetyStudyPlanDTO SelectById(int id) {
            var plan = context.SafetyStudyPlan.Where(x => x.Id == id)
                .Include(x => x.CreatedByNavigation)
                .Include(x => x.IdProjectTypeNavigation)
                .Include(x => x.IdTemplateNavigation)
                .Include(x => x.IdPlanTypeNavigation)
                .Include(x => x.ModifiedByNavigation)
                .FirstOrDefault();

            return MapEntityToDTO(plan);
        }

        public override int SelectCount() {
            return context.SafetyStudyPlan.Count();
        }

        public IQueryable<SafetyStudyPlanDTO> SelectAllPlans(IQueryable<SafetyStudyPlan> consulta) {
            return (from plan in consulta
                    from user in context.Users
                    select new SafetyStudyPlanDTO {
                        Activity = plan.PlanActivity,
                        CustomerName = plan.CustomerName,
                        ProducedBy = user.UserName,
                        UpdateDate = plan.UpdateDate ?? plan.CreateDate,
                        CompanyName = plan.CompanyName,
                        Id = plan.Id,
                        ProjectName = plan.ProjectName
                    });
        }

        public IQueryable<SafetyStudyPlan> SelectByWhere(Filter filter) {
            var consulta = (from plan in context.SafetyStudyPlan
                                //join user in context.Users on plan.CreatedBy equals user.Id
                            where
                            (filter == null || string.IsNullOrEmpty(filter.Activity) || plan.PlanActivity.Contains(filter.Activity)) &&
                            (filter == null || string.IsNullOrEmpty(filter.Title) || plan.ProjectName.Contains(filter.Title)) &&
                            (filter == null || string.IsNullOrEmpty(filter.ProducedBy) || plan.CreatedByNavigation.UserName.Contains(filter.ProducedBy)) &&

                            // Encontrar con que comparar
                            (filter == null || string.IsNullOrEmpty(filter.CheckedBy) || plan.PlanActivity.Contains(filter.CheckedBy)) &&
                            (filter == null || string.IsNullOrEmpty(filter.ApprovedBy) || plan.PlanActivity.Contains(filter.ApprovedBy)) &&

                            (filter == null || string.IsNullOrEmpty(filter.Organization) || plan.CompanyName.Contains(filter.Organization)) &&
                            (filter == null || string.IsNullOrEmpty(filter.ProducedFromDate) || plan.CreateDate >= Convert.ToDateTime(filter.ProducedFromDate)) &&
                            (filter == null || string.IsNullOrEmpty(filter.ProducedToDate) || plan.CreateDate <= Convert.ToDateTime(filter.ProducedToDate))
                            select plan);

            return consulta;
        }

        
        #region Mappers

        private SafetyStudyPlanDTO MapEntityToDTO(SafetyStudyPlan plan) {
            return new SafetyStudyPlanDTO() {
                Id = plan.Id,
                Activity = plan.PlanActivity,
                Aprover = plan.IdAproverNavigation != null ? new UserDTO() {
                    Id = plan.IdAproverNavigation.Id,
                    Name = plan.IdAproverNavigation.UserName
                } : null,
                Center = new CenterDTO() { Id = plan.IdCenter },
                CompanyName = plan.CompanyName,
                CreateDate = plan.CreateDate,
                CreatedBy = plan.CreatedByNavigation!= null ?  new UserDTO() {
                    Id = plan.CreatedByNavigation.Id,
                    Name = plan.CreatedByNavigation.UserName
                } : null,
                CustomerName = plan.CustomerName,
                PlanType = (!string.IsNullOrEmpty(plan.Type) ? SegurplanEnums.PlanType.Safety : (SegurplanEnums.PlanType?)null),
                ProducedBy = plan.CreatedByNavigation.UserName,//TODO: este quitarlo cuando quiten el campo
                ProjectType = new TypeDTO() {
                    Id = plan.IdProjectTypeNavigation.Id,
                    Name = plan.IdProjectTypeNavigation.Name
                },
                ProjectName = plan.ProjectName,
                Template = new TemplateDTO() {
                    Id = plan.IdTemplateNavigation.Id,
                    Name = plan.IdTemplateNavigation.Name,
                },
                Type = new TypeDTO() {
                    Id = plan.IdPlanTypeNavigation.Id,
                    Name = plan.IdPlanTypeNavigation.Name
                },
                UpdateDate = plan.UpdateDate,
                UpdatedBy = plan.ModifiedBy != null ? new UserDTO() {
                    Id = plan.ModifiedByNavigation.Id,
                    Name = plan.ModifiedByNavigation.UserName
                } : null
            };
        }

        #endregion

    }
}
