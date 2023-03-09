//using System;
//using System.Collections.Generic;

//namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
//    public partial class ChapterVersionInfo : AuditableTableBase {

//        public ChapterVersionInfo() {

//            Chapter = new HashSet<Chapter>();
//            ChapterVersion = new HashSet<ChapterVersion>();
//        }

//        public int Id { get; set; }

//        public int VersionNumber { get; set; }

//        public DateTime StartDate { get; set; }

//        public DateTime? EndDate { get; set; }

//        public ICollection<Chapter> Chapter { get; set; }

//        public ICollection<ChapterVersion> ChapterVersion { get; set; }
//    }
//}
