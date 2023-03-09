using AutoMapper;
using System.Linq;
using FluentValidation.Results;

namespace Segurplan.FrameworkExtensions.MediatR.Validation {
    
    public static class ValidationResultExtensions {
    
        public static ValidationResultWithTypeInfo For<T>(this ValidationResult validationResult) {
        
            return new ValidationResultWithTypeInfo(validationResult.Errors, typeof(T));
        }

        public static ValidationResult MapTo<TModel>(this ValidationResult validationResult, IMapper mapper) {
            
            if (!(validationResult is ValidationResultWithTypeInfo validationResultWithTypeInfo))
                return validationResult;

            var map = mapper.ConfigurationProvider.FindTypeMapFor(typeof(TModel), validationResultWithTypeInfo.ValidatedType);

            if (map == null) {
                return validationResult;
            }

            foreach (var error in validationResult.Errors) {
                var propertyMap = map.PropertyMaps.SingleOrDefault(p => p.DestinationName == error.PropertyName);

                if (propertyMap == null)
                    continue;

                error.PropertyName = propertyMap.SourceMember.Name;
            }

            return validationResult;
        }
    }
}
