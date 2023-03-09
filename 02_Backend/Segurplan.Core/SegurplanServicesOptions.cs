using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Segurplan.Core {
    public class SegurplanServicesOptions {
        private readonly List<Assembly> mapperProfilesAssemblies;
        private readonly List<Assembly> validatorsAssemblies;

        public SegurplanServicesOptions() {
            mapperProfilesAssemblies = new List<Assembly>();
            validatorsAssemblies = new List<Assembly>();
        }

        public IEnumerable<Assembly> MapperProfilesAssemblies => mapperProfilesAssemblies;

        public SegurplanServicesOptions AddMapperProfilesAssemblies(params Assembly[] assemblies) {
            mapperProfilesAssemblies.AddRange(assemblies);
            return this;
        }

        public IEnumerable<Assembly> ValidatorsAssemblies => validatorsAssemblies;
        public SegurplanServicesOptions AddValidatorsAssemblies(params Assembly[] assemblies) {
            validatorsAssemblies.AddRange(assemblies);
            return this;
        }
    }
}
