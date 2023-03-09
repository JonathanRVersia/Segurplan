using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ChapterActivityList.Export {
    public class ChapterArtivityToWordRequestHandler : IRequestHandler<ChapterArtivityToWordRequest, IRequestResponse<ChapterArtivityToWordResponse>> {
        #region CONSTANTS HEADER
        private const string Col_Chapter = "Capítulo";
        private const string Col_ChapterTitle = "Título del Capítulo";
        private const string Col_ReviewDate = "Fecha Revisión";
        private const string Col_RevisionNumber = "N° Revisión";
        #endregion
        #region CONSTANTS FONT
        private const string Verdana = "Verdana";
        #endregion
        public async Task<IRequestResponse<ChapterArtivityToWordResponse>> Handle(ChapterArtivityToWordRequest request, CancellationToken cancellationToken) {

            string exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            var local = Path.GetFullPath(Path.Combine(exePath, @"Templates\Exportar capitulos.docx"));

            var stream = new MemoryStream();

            byte[] byteArray = File.ReadAllBytes(local);

            using (stream = new MemoryStream()) {
                stream.Write(byteArray, 0, (int)byteArray.Length);

                using (WordprocessingDocument doc = WordprocessingDocument.Open(stream, true)) {

                    MainDocumentPart mainDocumentPart = doc.MainDocumentPart;

                    Table chapterTableHeader = new Table();
                    Table chapterTableDetail = new Table();

                    SetCurrentDate(doc);
                    SetHeaderStyles(chapterTableHeader);
                    SetDetailStyles(chapterTableDetail);
                    MapChapterHeaders(chapterTableHeader);
                    MapChaptersDetail(request, chapterTableDetail);

                    doc.MainDocumentPart.Document.Body.InsertBeforeSelf<Table>(chapterTableHeader);
                    doc.MainDocumentPart.Document.Body.InsertBeforeSelf<Table>(chapterTableDetail);

                }
                stream.Seek(0, SeekOrigin.Begin);
            }
            return RequestResponse.Ok(new ChapterArtivityToWordResponse(stream));
        }
        private static void SetCurrentDate(WordprocessingDocument doc) {
            foreach (var headerPart in doc.MainDocumentPart.HeaderParts) {

                foreach (var currentText in headerPart.RootElement.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>()) {
                    currentText.Text = currentText.Text.Replace("fecha_actual", DateTime.UtcNow.ToString("dd/MM/yyyy"));
                }
            }
        }
        private static void MapChaptersDetail(ChapterArtivityToWordRequest request, Table chapterTableDetail) {
            int i = 1;
            foreach (var chapter in request.Chapters) {

                TableRow row = new TableRow();

                var chapterVersionData = chapter.ChapterVersion.Where(x => x.ApprovementDate < DateTime.Now && x.EndDate == null ||
                                                                           x.EndDate > DateTime.Now)
                                                               .Select(x => new { x.VersionNumber, x.ApprovementDate })
                                                               .OrderByDescending(x => x.VersionNumber)
                                                               .FirstOrDefault();

                if (chapterVersionData != null) {

                    var chapterId = new TableCell(SetVerticalAlignmentCenter(), new Paragraph(SetJustificationParagraph(JustificationValues.Center), new Run(SetVerdanaFont(), new Text(string.Format("{0}", i)))));
                    var chapterTitle = new TableCell(SetVerticalAlignmentCenter(), new Paragraph(new Run(SetVerdanaFont(), new Text(chapter.Title))));
                    var chapterVersion = new TableCell(SetVerticalAlignmentCenter(), new Paragraph(SetJustificationParagraph(JustificationValues.Center), new Run(SetVerdanaFont(), new Text(chapterVersionData.ApprovementDate.HasValue ? chapterVersionData.ApprovementDate.Value.ToString("dd/MM/yyyy") : null))));
                    var chapterNumber = new TableCell(SetVerticalAlignmentCenter(), new Paragraph(SetJustificationParagraph(JustificationValues.Center), new Run(SetVerdanaFont(), new Text(chapterVersionData.VersionNumber.ToString()))));

                    row.Append(chapterId, chapterTitle, chapterVersion, chapterNumber);

                    chapterTableDetail.AppendChild(row);
                }
                i++;
            }
        }
        private static TableCellProperties SetVerticalAlignmentCenter() => new TableCellProperties(new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center });
        private static RunProperties SetVerdanaFont() => new RunProperties(new RunFonts() { Ascii = Verdana, HighAnsi = Verdana, ComplexScript = Verdana }, new FontSize() { Val = "18" });
        private static ParagraphProperties SetJustificationParagraph(JustificationValues justification) => new ParagraphProperties(new Justification() { Val = justification });
        private static void SetHeaderStyles(Table chapterTable) {
            TableProperties tblProp = new TableProperties(
                new TableBorders(
                    new TopBorder() { Val = SetSingleBorder() },
                    new BottomBorder() { Val = SetSingleBorder() },
                    new InsideHorizontalBorder() { Val = SetSingleBorder() }
                ),
                new TableCellMarginDefault(
                    new TopMargin() { Width = "30", Type = TableWidthUnitValues.Dxa },
                new BottomMargin() { Width = "30", Type = TableWidthUnitValues.Dxa }
                )
            );
            chapterTable.AppendChild<TableProperties>(tblProp);
        }
        private static EnumValue<BorderValues> SetSingleBorder() => new EnumValue<BorderValues>(BorderValues.Single);
        private static void SetDetailStyles(Table chapterTableDetail) {
            TableProperties tblProp = new TableProperties(
                new TableCellMarginDefault(
                    new StartMargin() { Width = "50", Type = TableWidthUnitValues.Dxa },
                    new TopMargin() { Width = "70", Type = TableWidthUnitValues.Dxa }
                )
            );

            chapterTableDetail.AppendChild<TableProperties>(tblProp);
        }
        private static void MapChapterHeaders(Table chapterTable) {
            TableRow row = new TableRow();

            TableCellProperties tcp = new TableCellProperties(
                new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center },
                new MarginHeight() { Val = 100 }
                );

            var chapter = new TableCell(new Paragraph(SetJustificationParagraph(JustificationValues.Center), new Run(SetVerdanaFont(), new Text(Col_Chapter))));
            var chapterTitle = new TableCell(new Paragraph(new Run(SetVerdanaFont(), new Text(Col_ChapterTitle))));
            var reviewDate = new TableCell(new Paragraph(SetJustificationParagraph(JustificationValues.Center), new Run(SetVerdanaFont(), new Text(Col_ReviewDate))));
            var revisionNumber = new TableCell(new Paragraph(SetJustificationParagraph(JustificationValues.Center), new Run(SetVerdanaFont(), new Text(Col_RevisionNumber))));

            chapter.Append(tcp);
            chapterTitle.Append(tcp.CloneNode(true));
            reviewDate.Append(tcp.CloneNode(true));
            revisionNumber.Append(tcp.CloneNode(true));

            row.Append(chapter, chapterTitle, reviewDate, revisionNumber);

            chapterTable.AppendChild(row);
        }
    }
}
