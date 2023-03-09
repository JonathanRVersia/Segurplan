using Segurplan.FrameworkExtensions.Query;

namespace Segurplan.Core.Actions.Administration.Seriousness.ModalList.Specifications {
    public class SeriousnessSpecification : Specification<SeriousnessListResponse.ListItem> {

        public void ById(int id) => Criteria(pm => pm.Id == id);

        public void ByValue(string value) => Criteria(pm => pm.Value.Contains(value));
    }
}
