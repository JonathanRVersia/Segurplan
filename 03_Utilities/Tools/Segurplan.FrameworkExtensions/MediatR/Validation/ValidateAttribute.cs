using System;

namespace Segurplan.FrameworkExtensions.MediatR.Validation {
   
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ValidateAttribute : Attribute {
    
        public ValidateAttribute(string propertyName) {
        
            PropertyName = propertyName;
        }

        public string PropertyName { get; }
    }
}
