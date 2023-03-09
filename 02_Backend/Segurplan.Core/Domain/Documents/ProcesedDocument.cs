using System.IO;

namespace Segurplan.Core.Domain.Documents {
    public class ProcesedDocument {
        public ProcesedDocument(MemoryStream responseStream, string outputFileName, string v) {
            ResponseStream = responseStream;
            OutputFileName = outputFileName;
            MediaType = v;
        }

        public MemoryStream ResponseStream { get; }
        public string OutputFileName { get; set; }

        public string MediaType { get; }
    }
}
