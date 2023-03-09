using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Segurplan.Core.Actions.RiskEvaluation.EvaluationsOfRisksAndPreventiveMeasures.Generate.Models;
using Segurplan.Core.Domain.Documents;

namespace Segurplan.Core.Helpers {
    public static class ZipBuilder {

        private const string ZipMediaType = "application/zip";

        /// <summary>
        /// Store on zip file one file 
        /// </summary>
        /// <param name="buffer">The file to store</param>
        /// <param name="fileTitle">Name of the file</param>
        /// <returns>Return file as byte[],in the handler put "return File(ms.ToArray(), "application/zip", "Images.zip");" to return the zip </returns>
        public static byte[] ToZip(byte[] buffer, string fileTitle) {
            using (var ms = new MemoryStream()) {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true)) {

                    var zipEntry = archive.CreateEntry(fileTitle, CompressionLevel.Fastest);

                    using (var zipStream = zipEntry.Open()) {
                        zipStream.Write(buffer, 0, buffer.Length);
                    }
                }
                return ms.ToArray();

            }

        }

        /// <summary>
        /// Store on zip file multiple files
        /// </summary>
        /// <param name="files">Dictionary of files , key => file as byte[], value => filename as string </param>
        /// <returns>Return file as byte[],in the handler put "return File(ms.ToArray(), "application/zip", "Images.zip");" to return the zip </returns>
        public static byte[] ToZipRange(Dictionary<byte[], string> files) {
            using (var ms = new MemoryStream()) {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true)) {

                    foreach (var file in files) {

                        var zipEntry = archive.CreateEntry(file.Value, CompressionLevel.Fastest);

                        using (var zipStream = zipEntry.Open()) {
                            zipStream.Write(file.Key, 0, file.Key.Length);
                        }
                    }

                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Store on zip file multiple files
        /// </summary>
        /// <param name="files">Dictionary of files , key => file as byte[], value => filename as string </param>
        /// <returns>Return file as memory stream and media type</returns>
        public static (MemoryStream, string, string) ToZipMemoryStreamRange(List<ProcesedDocument> files) {
            using (var ms = new MemoryStream()) {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true)) {

                    foreach (var file in files) {

                        var zipEntry = archive.CreateEntry(file.OutputFileName, CompressionLevel.Fastest);

                        using (var zipStream = zipEntry.Open()) {
                            zipStream.Write(file.ResponseStream.ToArray(), 0, file.ResponseStream.ToArray().Length);
                        }
                    }

                }
                return (new MemoryStream(ms.ToArray()), ZipMediaType, ".zip");
            }
        }
    }
}

