using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ActivityReservation.Common
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGoogleRecaptchaHelper(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.TryAddSingleton<GoogleRecaptchaHelper>();
            return services;
        }

        public static IServiceCollection AddGoogleRecaptchaHelper(this IServiceCollection services, Action<GoogleRecaptchaOptions> action)
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
            return services.AddGoogleRecaptchaHelper();
        }

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
            return services.AddGoogleRecaptchaHelper();
        }
    }
}
