using Segurplan.Core.Actions.Plans.PlanManagement.Files.View;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Files {
    public class FilesProfile : AutoMapper.Profile {

        public FilesProfile() {

            AddViewFilesProfiles();

        }

        private void AddViewFilesProfiles() {
            CreateMap<SafetyStudyPlanPlaneFile, ViewPlanPlaneItem>();
            CreateMap<Plane, ViewPlanPlaneItem>();
        }
    }
}
