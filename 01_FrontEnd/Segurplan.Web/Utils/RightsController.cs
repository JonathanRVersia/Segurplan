using System.Collections.Generic;

namespace Segurplan.Web.Utils {
    public static class RightsController {
        private const int AdministratorRole = 1;

        public static List<int> UserRoles { get; set; }

        /// <summary>
        /// Checks whether a given user can edit or not a plan
        /// </summary>
        /// <param name="planOwner">The plan owner identifier</param>
        /// <param name="userId">The user identifier</param>
        /// <param name="roleList">The roles of the user</param>
        /// <returns>Whether user can edit a plan or not</returns>
        public static bool CanEditPlan(int planOwner, int userId) {
            return planOwner == userId || UserRoles.Contains(AdministratorRole);
        }

        public static bool ImAAdmin() {
            return UserRoles != null
                ? UserRoles.Contains(AdministratorRole)
                : false;
        }
    }
}
