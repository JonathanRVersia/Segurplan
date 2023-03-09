using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Segurplan.Core.Domain.Documents;

namespace Segurplan.Core.Actions.RiskEvaluation.EvaluationsOfRisksAndPreventiveMeasures.Generate.RiskMap {
    public class GenerateEvaluationOfRisksDocsRequestResponse {

        //public byte[] file;
        public MemoryStream ResponseStream { get; }
        public string OutputFileName { get; }

        public string MediaType { get; }

        //public GenerateEvaluationOfRisksDocsRequestResponse(byte[] file) {
        //    this.file = file;
        //}
        public GenerateEvaluationOfRisksDocsRequestResponse(MemoryStream responseStream, string outputFileName, string mediaType) {
            ResponseStream = responseStream;
            OutputFileName = outputFileName;
            MediaType = mediaType;
        }
    }
}
