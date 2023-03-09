
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class Profile {
        public Profile() {
            SafetyStudyPlan = new HashSet<SafetyStudyPlan>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        [InverseProperty("IdCreatorProfileNavigation")]

        public ICollection<SafetyStudyPlan> SafetyStudyPlan { get; set; }
    }
}
