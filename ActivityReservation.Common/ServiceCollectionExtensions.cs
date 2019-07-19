using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ActivityReservation.Common
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTencentCaptchaHelper(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.TryAddSingleton<TencentCaptchaHelper>();
            return services;
        }

        public static IServiceCollection AddTencentCaptchaHelper(this IServiceCollection services, Action<TencentCaptchaOptions> action)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            services.Configure(action);
            return services.AddTencentCaptchaHelper();
        }
    }
}
