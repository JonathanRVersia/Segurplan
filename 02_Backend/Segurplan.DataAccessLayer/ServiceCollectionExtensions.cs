using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Segurplan.DataAccessLayer.DataAccessManagers;

namespace Segurplan.DataAccessLayer {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddSegurplanDAL(this IServiceCollection services) {
            foreach (var damType in Assembly.GetExecutingAssembly().GetTypes()
                                            .Where(x => x.Namespace == typeof(SafetyStudyPlanDam).Namespace).ToList()) {
                services.AddScoped(damType);
            }
            return services;
        }
    }
}
