using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Segurplan.Core.Domain.Documents {

    /// <summary>
    /// Service for summernote html strings satinization
    /// </summary>
    public class HtmlStringCleaner : IHtmlStringCleaner {
        private const string MatchContentBetweenCloseAndOpenTag = "(?<=>)(.*?)(?=<)"; //Buscar un nombre mejor
        private const string MatchColgroupAndContent = "<colgroup>(.*?)<\\/colgroup>";
        public const string MatchDotBetweenCloseTagAndSpace = "(?<=>)·\\s";
        public const string MatchExtraSemicolonAndQuot = "(?<=\");\"";
        public const string MatchStyle = "(style=\")(.*?)(\\\")";
        public const string MatchAllFontSize = "(font-size:)(.*?)(?=;|\")";
        public const string MatchTableContent = "<td(.*?)td>";

        public Dictionary<string, string> HtmlCleanRegexPatternsAndReplacements { get; set; } = new Dictionary<string, string>();


        public HtmlStringCleaner() {
            HtmlCleanRegexPatternsAndReplacements.Add("(<img[^>]+)(?<!/)>", "$1/>");
            //HtmlCleanRegexPatternsAndReplacements.Add("font-size:[^;']*(;)?", string.Empty);
            HtmlCleanRegexPatternsAndReplacements.Add("(font-family:)(.*?)(?=;|\")/g", "font-family:Verdana, sans-serif");
            //Piilla lo que está entre > y < HtmlCleanRegexPatternsAndReplacements.Add("(?<=>)(.*?)(?=<)", ""+"");
        }

        public string Sanitize(string html, string key) {
            //Limpieza de html con tablas
            //Falla si hay coincidencia entre el texto de las tablas y algún otro elemento del html, por ejemplo
            //TODO: Comprobar si es necesario o no
            //html = AddSpanToTableValues(html);

            //OJO: Provisional para dar estilo a WorkDescriptionHtml (Tala azul)
            //TODO: Cuando tengamos una BD con todos los workDetails habrá que modificar esto y ponerlo en seed para darle el formato a su html al generar la nueva BD      
            //html = DarEstiloTablaAzul(html, key);

            
            html = BasicTagClean(html);
            html = RunReplaceClear(html);
            html = AddBullets(html);
            if (html.Contains("<table")) {
                html = CleanTable(html);
            }
            html = SummernoteTableStyle(html);
            html = HtmlFijoClear(html);
            html = HtmlDinamicoClear(html);
            html = RunRegexClear(html);
            html = FontRemplaceClear(html);

            //Eliminar los saltos de linea restantes
            html = Regex.Replace(html, "(\r\n|\n|\r)", string.Empty);
            html = RemoveExtraSpaces(html);
            //html = AddSpanToTableValues(html, key);
            return html;
        }

        public string BasicTagClean(string html) {

            //Eliminar saltos de linea
            html = Regex.Replace(html, "(\r\n|\n|\r)", string.Empty);

            //Eliminar tags innecesarios
            html = Regex.Replace(html, "((<a)(.*?)(\">))", "<b>");
            html = Regex.Replace(html, "</a>", "</b>");
            html = Regex.Replace(html, "((<div)(.*?)(>))", "<p>");
            html = Regex.Replace(html, "</div>", "</p>");
            html = Regex.Replace(html, "((<!)(.*?)(>))", string.Empty);
            html = Regex.Replace(html, "((<o:p)(.*?)(>))", string.Empty);
            html = Regex.Replace(html, "</o:p>", string.Empty);
            html = Regex.Replace(html, "((<v:)(.*?)(>))", string.Empty);
            html = Regex.Replace(html, "((</v:)(.*?)(>))", string.Empty);
            html = Regex.Replace(html, "((v:)(.*?)(=))", "name=");
            html = Regex.Replace(html, "((<span)(.*?)(>))", string.Empty);
            html = html.Replace("</span>", string.Empty);
            html = html.Replace("<span>", string.Empty);

            //Simplificamos tags
            html = Regex.Replace(html, "((<br)(.*?)(>))", "<{Reserved}br>");
            html = Regex.Replace(html, "((<b)(.*?)(>))", "<b>");

            //Simplificamos las <p>
            html = Regex.Replace(html, "((<p)(.*?)(>))", "<p>");

            return html;
        }

        public string RemoveExtraSpaces(string html) {
            while (html.Contains("  ")) {
                html = html.Replace("  ", " ");
            }
            return html;
        }
        
        public string CleanTable(string stringToClean) {
            stringToClean = Regex.Replace(stringToClean, "((<table)(.*?)(>))", "<table notDone>");
            stringToClean = Regex.Replace(stringToClean, "((</table)(.*?)(>))", "</table notDone>");
            while(stringToClean.Contains("<table notDone>")) {
                int startPoint = stringToClean.IndexOf("<table notDone>");
                int endPoint = stringToClean.IndexOf("</table notDone>") + "</table notDone>".Length;
                var currentTable = stringToClean.Substring(startPoint, endPoint - startPoint);
                var noChangedTable = currentTable;
                currentTable = Regex.Replace(currentTable, "((<tr)(.*?)(>))", "<tr>");
                currentTable = Regex.Replace(currentTable, "((<td)(.*?)(>))", "<td style=\"border:1px solid black;font-family:Verdana; font-size:9pt\">");
                currentTable = Regex.Replace(currentTable, "((<table)(.*?)(>))", "<table class=\"table table-bordered\" style=\"border:1px solid black\">");
                currentTable = Regex.Replace(currentTable, "((</table)(.*?)(>))", "</table>");
                currentTable = currentTable.Replace("&quot;", string.Empty);
                currentTable = currentTable.Replace("&nbsp;", string.Empty);
                currentTable = currentTable.Replace("<sub>", string.Empty);
                currentTable = currentTable.Replace("</sub>", string.Empty);
                currentTable = currentTable.Replace("> ", ">");
                currentTable = currentTable.Replace("< ", "<");
                currentTable = Regex.Replace(currentTable, MatchColgroupAndContent, string.Empty);
                var regCurrentTable = Regex.Matches(currentTable, MatchTableContent);
                MatchCollection regex;
                List<string> doneList = new List<string>();
                doneList.Add(" ");
                foreach (var tableData in regCurrentTable) {
                    string tableDataString = tableData.ToString();
                    regex = Regex.Matches(tableDataString, MatchContentBetweenCloseAndOpenTag);
                    foreach (var reg in regex) {
                        string regstring = reg.ToString();
                        if (!doneList.Contains(regstring)) {
                            if (regstring != string.Empty) {
                                regstring = regstring.Replace("(", "\\(");
                                regstring = regstring.Replace(")", "\\)");
                                currentTable = Regex.Replace(currentTable, $"(?<=>)({regstring})(?=<)", $"<span>{regstring}</span>");
                                currentTable = currentTable.Replace("\\(", "(");
                                currentTable = currentTable.Replace("\\)", ")");
                                doneList.Add(reg.ToString());
                            }
                        }
                    }
                }
                stringToClean = stringToClean.Replace(noChangedTable, currentTable);
            }

            return stringToClean;
        }

        private string AddBullets(string html) {
            try {
                html = Regex.Replace(html, "((<ul)(.*?)(>))", "<ul>");
                html = Regex.Replace(html, "((<ol)(.*?)(>))", "<ol>");
                html = Regex.Replace(html, "((<li)(.*?)(>))", "<li>");

                html = RemoveExtraSpaces(html);

                //Cambiar las OrderedList por un orden numerico
                #region OrderedList
                while (html.Contains("<ol>")) {
                    int pointFrom = html.IndexOf("<ol>");
                    int pointTo = html.IndexOf("</ol>") + "</ol>".Length;
                    var resultOrderedList = html.Substring(pointFrom, pointTo - pointFrom);
                    var changedResult = Regex.Replace(resultOrderedList, @"((<li)(.*?)(>))", "<p style=\"line-height: 1.6; margin: 9pt 9pt 9pt 30pt; font-size:9pt; font-family:Verdana;\"><span>/%NUMERO%/   </span>");
                    changedResult = changedResult.Replace("</li>", "</p>");

                    var num = new List<int>();
                    while (changedResult.Contains("/%NUMERO%/")) {
                        num.Add(0);
                        int numberPosition = changedResult.IndexOf("/%NUMERO%/");
                        changedResult = changedResult.Remove(numberPosition, "/%NUMERO%/".Length).Insert(numberPosition, num.Count.ToString() + ".");
                    }
                    changedResult = changedResult.Replace("<ol>", string.Empty);
                    changedResult = changedResult.Replace("</ol>", string.Empty);
                    html = html.Replace(resultOrderedList, changedResult);
                }
                #endregion

                //Cambiar las UnorderedList por bullets
                #region UnorderedList  
                html = Regex.Replace(html, @"((<li)(.*?)(>))", "<p style=\"line-height: 1.6; margin: 9pt 9pt 9pt 30pt; font-size:9pt; font-family:Verdana;\"><b>\u2022   </b>");
                html = html.Replace("</li>", "</p>");
                html = html.Replace("<ul>", string.Empty);
                html = html.Replace("</ul>", string.Empty);
                #endregion

                return html;
            } catch (System.Exception ex) {

                throw ex;
            }     
        }
        //OJO: Provisional para dar estilo a WorkDescriptionHtml (Tala azul)
        //Cuando tengamos una BD con todos los workDetails habrá que modificar esto y ponerlo en seed para darle el formato a su html al generar la nueva BD
        private string DarEstiloTablaAzul(string html, string key) {
            if (key == "WordDescriptionHtml" && !html.Contains("<p>") || key == "WorkDetailsHtml" && !html.Contains("<p>")) {
                if (!html.Contains("<div>")) {
                    html = $"<p style =\"font-size:9pt; font-family:Verdana, sans-serif\"> {html} </p> ";
                    html = html.Replace("\\*", "<br>");
                }
                html = html.Replace("<div>", "<p style=\"font-size:9pt; font-family:Verdana, sans-serif\">");
                html = html.Replace("</div>", "</p>");
            }
            return html;
        }

        private string SummernoteTableStyle(string html) {
            if (html.Contains("td style=\"border: 1px solid black;"))
                html = html.Replace("td style=\"border: 1px solid black;", "td style=\"border: 1pt solid black; font-size: 9pt");

            return html;
        }


        private string AddSpanToTableValues(string html, string key) {
            if (html.Contains("<tr") && key != "EmergencyPlanDescriptionHtml") {
                html = html.Replace("&quot;", string.Empty);
                html = html.Replace("&nbsp;", string.Empty);
                html = html.Replace("<sub>", string.Empty);
                html = html.Replace("</sub>", string.Empty);
                html = Regex.Replace(html, "<strong(.*?)>", string.Empty);
                html = html.Replace("</strong>", string.Empty);
                html = Regex.Replace(html, MatchColgroupAndContent, string.Empty);
                var regTable = Regex.Matches(html, MatchTableContent);
                MatchCollection regex;
                //if (regTable != null)
                //    regex = Regex.Matches(regTable[0].ToString(), MatchContentBetweenCloseAndOpenTag);
                //else

                //var regex = Regex.Matches(html, MatchContentBetweenCloseAndOpenTag);
                List<string> hechos = new List<string>();
                foreach (var reg in regTable) {                    
                    regex = Regex.Matches(reg.ToString(), MatchContentBetweenCloseAndOpenTag);
                    foreach (var x in regex) {             
                        string regstring = x.ToString();
                        if (!hechos.Contains(regstring)) {
                            if (regstring != string.Empty) {
                                regstring = regstring.Replace("(", "\\(");
                                regstring = regstring.Replace(")", "\\)");
                                html = Regex.Replace(html, $"(?<=>)({regstring})(?=<)", $"<span>{regstring}</span>");
                                html = html.Replace("\\(", "(");
                                html = html.Replace("\\)", ")");

                                hechos.Add(x.ToString());
                                //html = Regex.Replace(html, $"({regstring})", $"<span>{regstring}</span>");//Añade un <span></span> al texto de la tabla
                            }
                        }
                    }
                }
            }
            if (html.Contains("td style=\"border: 1px solid black;"))
                html = html.Replace("td style=\"border: 1px solid black;", "td style=\"border: 1pt solid black; font-size: 9pt");

            return html;
        }

        private string HtmlFijoClear(string html) {
            return html
            .Replace(".0", string.Empty)
            .Replace("Helvetica,serif", "Verdana, sans-serif");
        }

        private string HtmlDinamicoClear(string html) {
            return html
            .Replace("<p>", "<p style=\"font-size:9pt; font-family:Verdana, sans-serif\">")
            .Replace("font-size:12", "font-size:9");
        }

        public string RunReplaceClear(string html) {
            return html
                .Replace("&amp;", "&")
                .Replace("&nbsp;", " &nbsp; ")
                .Replace(" &nbsp; &nbsp; &nbsp; &nbsp; ", "<pre>     </pre>")
                .Replace("&nbsp;", System.Environment.NewLine)
                .Replace("<{Reserved}br>", "<br/>")
                .Replace("<hr>", "<hr/>")
                .Replace("&quot;", string.Empty)
                .Replace("&lt;", "~lt;")
                .Replace("&gt;", "~gt;")
                .Replace("&#", "~#")
                .Replace("&", "&amp;")
                .Replace("~lt;", "&lt;")
                .Replace("~gt;", "&gt;")
                .Replace("~#", "&#")
                .Replace("mso-ansi-language:ES-TRAD", "")
                .Replace("lang=\"ES-TRAD\"", "");
        }

        public string FontRemplaceClear(string html) {
            return html
                .Replace("font-size:8,5pt", "font-size:9pt")
                .Replace("font-family:&quot;", "font-family:")
                .Replace("font-family: &quot;", "font-family:")
                .Replace("&quot;,sans-serif", ", sans-serif")
                .Replace("&quot; ,sans-serif", ", sans-serif")
                .Replace("font-family:\"", "font-family:")
                .Replace("font-family: \"", "font-family:")
                .Replace("\", sans-serif", ", sans-serif")
                .Replace("\",sans-serif", ", sans-serif")
                //.Replace("Microsoft Sans Serif", "Verdana, sans-serif")
                .Replace("Times New Roman&quot;", "Times New Roman")
                .Replace("Times New Roman", "Helvetica");
        }

        public string RunRegexClear(string html) {
            foreach (var regex in HtmlCleanRegexPatternsAndReplacements)
                html = Regex.Replace(html, regex.Key, regex.Value);
            return html;
        }
    }
}
