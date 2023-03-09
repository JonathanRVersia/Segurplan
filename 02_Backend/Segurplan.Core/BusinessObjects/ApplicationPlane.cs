namespace Segurplan.Core.BusinessObjects {
    public class ApplicationPlane {

        public int Id { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        //public byte[] Data { get; set; }

        public bool IsAvailable { get; set; } 

        public bool HasFile { get; set; }
    }
}
