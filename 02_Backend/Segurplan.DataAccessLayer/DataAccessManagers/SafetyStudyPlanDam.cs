using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.DataAccessManagers.DamBase;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.DataAccessLayer.DataAccessManagers {
    public class SafetyStudyPlanDam : SegurplanDamBase {
        public SafetyStudyPlanDam(SegurplanContext context) : base(context) {
        }

      
        
        public IQueryable<SafetyStudyPlan> SelectByWhere(Expression<Func<SafetyStudyPlan, bool>> expression) {
            return context.SafetyStudyPlan.Where(expression);
        }

        public async Task<List<int>> MyRoles(int currentUserId) {
            try {
                return (from x in context.UserRoles
                        where x.UserId == currentUserId
                        select x.RoleId).ToList<int>();
            } catch (Exception e) {
                Debug.WriteLine(e.ToString());
                return null;
            }

        }                   
    }
}
