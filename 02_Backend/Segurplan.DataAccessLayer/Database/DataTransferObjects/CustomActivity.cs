
namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class CustomActivity : AuditableTableBase {

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public CustomSubchapter CustomSubchapter { get; set; }

        public int? CustomSubchapterId { get; set; }

        public SubChapter SubChapter {get;set;}

        public int? SubChapterId { get; set; }
    }
}
