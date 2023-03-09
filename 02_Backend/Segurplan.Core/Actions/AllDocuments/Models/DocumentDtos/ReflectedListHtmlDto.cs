using System.Collections.Generic;

namespace Segurplan.Core.Actions.AllDocuments.Models.DocumentDtos {
    public class ReflectedListHtmlDto {
        public ReflectedListHtmlDto(string typeName, string propertyName) {
            TypeName = typeName;
            PropertyName = propertyName;
        }
        /// <summary>
        /// Reflected type name for filters
        /// </summary>
        public string TypeName { get; }

        /// <summary>
        /// Reflected property name for filters
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Object property html values as dicctionary
        /// </summary>
        public Dictionary<string, string> PropertyValues { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Object collection properties as collection of ReflectedListHtmlDto
        /// </summary>
        public List<ReflectedListHtmlDto> Sons { get; set; } = new List<ReflectedListHtmlDto>();

    }
}
