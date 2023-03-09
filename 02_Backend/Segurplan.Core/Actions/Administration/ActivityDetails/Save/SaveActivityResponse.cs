namespace Segurplan.Core.Actions.Administration.ActivityDetails.Save {
    public class SaveActivityResponse {
        public int SavedId { get; set; }

        public SaveActivityResponse(int savedId) {
            SavedId = savedId;
        }
    }
}
