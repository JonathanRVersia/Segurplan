using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Segurplan.FrameworkExtensions.Query;

namespace Segurplan.Core.Actions.Administration.PreventiveMeasures.ModalList.Specifications {
    public class PreventiveMeasuresSpecification : Specification<PreventiveMeasureListResponse.ListItem> {

        public void ByCode(string code) => Criteria(pm => pm.Code.Contains(code));

        public void ByDescription(string description) => Criteria(pm => pm.Description.Contains(description));

        public void ByInitialWord(string initialWord) => Criteria(pm => Regex.Replace(pm.Description.ToLower(), "<.*?>", String.Empty).ToString().StartsWith(initialWord.ToLower()));


        
        private string RemoveAccents(string value) {
            return new string(value.Normalize(NormalizationForm.FormD).Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray());
        }
    }
}
