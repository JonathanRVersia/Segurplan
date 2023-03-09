using System;

namespace Segurplan.Core.BusinessObjects {
    public enum AppPlanFileType {
        Anagram = 1
    }

    public class PlanFile {
        private string size = string.Empty;

        public int Id { get; set; }

        public string Name { get; set; }

        public byte[] Data { get; set; }

        public decimal DataLength { get; set; }

        public string FileSize { get => GetSize(DataLength); set => size = GetSize(DataLength); }

        public bool DefaultFile { get; set; }

        private string GetSize(decimal dataLength) {
            var fileSize = "0 Byte";
            double bytes = Decimal.ToDouble(dataLength);

            var sizes = new string[5] { "Bytes", "KB", "MB", "GB", "TB" };

            if (bytes != 0) {
                var i = Convert.ToInt32(Math.Floor(Math.Log(bytes)) / Math.Log(1024));

                fileSize = $"{Math.Round(bytes / Math.Pow(1024, i), 0)}  {sizes[i]}";
            }

            return fileSize;
        }
    }
}
