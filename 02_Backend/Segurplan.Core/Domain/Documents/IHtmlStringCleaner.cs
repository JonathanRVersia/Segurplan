using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.Core.Domain.Documents {
    /// <summary>
    /// Service for summernote html strings satinization
    /// </summary>
    public interface IHtmlStringCleaner {
        Dictionary<string, string> HtmlCleanRegexPatternsAndReplacements { get; set; }
        

        string Sanitize(string html, string key);
        string RunReplaceClear(string html);
        string RunRegexClear(string html);
    }
}
