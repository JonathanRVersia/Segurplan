namespace Segurplan.Core.Actions.AllDocuments.Models.DocumentDtos {

    public class PreventiveMeasureListDocumentDto {
        public int Id { get; set; }
        public string PreventiveMeasureDescriptionHtml { get; set; } = "";

        public int PreventiveMeasureOrder { get; set; }
    }
}
