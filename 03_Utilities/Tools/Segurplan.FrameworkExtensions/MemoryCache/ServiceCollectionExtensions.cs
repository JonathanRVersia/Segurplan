using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Segurplan.FrameworkExtensions.MemoryCache {
    public static class ServiceCollectionExtensions {

        public static IServiceCollection AddMemoryCacheTools(this IServiceCollection services) {

            services.AddSingleton<MemoryCacheService>();

            return services;
        }
    }
}
