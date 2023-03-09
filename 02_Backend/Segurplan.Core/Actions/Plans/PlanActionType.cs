

namespace Segurplan.Core.Actions.Plans {
    public enum PlanActionType {
        View = 1,
        Edit,
        Create,
        Duplicate,
        Delete,
        Update,
        UpdateAndCreate
    }

    public enum PlanTab {
        GeneralData,
        Activities,
        AdditionalData,
        Budget,
        OtherDocuments
    }
}
