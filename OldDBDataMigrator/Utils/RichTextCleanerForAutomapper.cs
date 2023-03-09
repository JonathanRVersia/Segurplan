
using System.Text.RegularExpressions;

namespace OldDBDataMigrator.Utils {
    public static class RichTextCleanerForAutomapper {

        public static string CleanRichText(string textToClean) {

            if (textToClean != null) {
                textToClean = Regex.Replace(textToClean, @"\{\*?\\[^{}]+}|[{}]|\\\n?[A-Za-z]+\n?(?:-?\d+)?[ ]?", "");

                if (textToClean.Contains("\r"))
                    textToClean = textToClean.Replace("\r", "");

                if (textToClean.Contains("\n"))
                    textToClean = textToClean.Replace("\n", "");

                if (textToClean.StartsWith(" "))
                    textToClean = textToClean.Substring(1, textToClean.Length - 1);

                if (textToClean.Contains("\\'e1"))
                    textToClean = textToClean.Replace("\\'e1", "á");

                if (textToClean.Contains("\\'e9"))
                    textToClean = textToClean.Replace("\\'e9", "é");

                if (textToClean.Contains("\\'ed"))
                    textToClean = textToClean.Replace("\\'ed", "í");

                if (textToClean.Contains("\\'f3"))
                    textToClean = textToClean.Replace("\\'f3", "ó");

                if (textToClean.Contains("\\'fa"))
                    textToClean = textToClean.Replace("\\'fa", "ú");

                if (textToClean.Contains("\\'f1"))
                    textToClean = textToClean.Replace("\\'f1", "ñ");

                if (textToClean.Contains("\\'b7"))
                    textToClean = textToClean.Replace("\\'b7", " ");

                if (textToClean.Contains("\\'c1"))
                    textToClean = textToClean.Replace("\\'c1", "Á");

                if (textToClean.Contains("\\'c9"))
                    textToClean = textToClean.Replace("\\'c9", "É");

                if (textToClean.Contains("\\'cd"))
                    textToClean = textToClean.Replace("\\'cd", "Í");

                if (textToClean.Contains("\\'d3"))
                    textToClean = textToClean.Replace("\\'d3", "Ó");

                if (textToClean.Contains("\\'da"))
                    textToClean = textToClean.Replace("\\'da", "Ú");

                if (string.IsNullOrEmpty(textToClean))
                    textToClean = null;
            }

            return textToClean;
        }
    }
}
