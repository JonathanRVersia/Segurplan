namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class ActivityRelations {

        public int Id { get; set; }

        public int IdRelations { get; set; }

        public int? IdChapterRelation { get; set; }

        public int? IdSubchapterRelation { get; set; }

        public int? IdActivityRelation { get; set; }

    }
}
