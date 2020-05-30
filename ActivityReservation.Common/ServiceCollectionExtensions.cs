using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WeihanLi.Common.Http;

namespace ActivityReservation.Common
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGoogleRecaptchaHelper(this IServiceCollection services, Action<HttpClient> clientOptions = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (null == clientOptions)
            {
                services.AddHttpClient<GoogleRecaptchaHelper>();
            }
            else
            {
                services.AddHttpClient<GoogleRecaptchaHelper>(clientOptions);
            }
            services.TryAddSingleton<GoogleRecaptchaHelper>();
            return services;
        }

        public static IServiceCollection AddGoogleRecaptchaHelper(this IServiceCollection services, IConfiguration configuration, Action<HttpClient> clientOptions = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (null != configuration)
            {
                services.Configure<GoogleRecaptchaOptions>(configuration.Bind);
            }
            return services.AddGoogleRecaptchaHelper(clientOptions);
        }

        public static IServiceCollection AddTencentCaptchaHelper(this IServiceCollection services, Action<HttpClient> clientConfigure = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (null != clientConfigure)
            {
                services.AddHttpClient<TencentCaptchaHelper>(clientConfigure)
                    .ConfigurePrimaryHttpMessageHandler(() => new NoProxyHttpClientHandler());
            }
            else
            {
                services.AddHttpClient<TencentCaptchaHelper>()
                    .ConfigurePrimaryHttpMessageHandler(() => new NoProxyHttpClientHandler());
            }
            services.TryAddSingleton<TencentCaptchaHelper>();
            return services;
        }

        public static IServiceCollection AddTencentCaptchaHelper(this IServiceCollection services, Action<TencentCaptchaOptions> action, Action<HttpClient> clientConfigure = null)
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

        public static IServiceCollection AddGiteeStorageProvider(this IServiceCollection services,
            IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (null != configuration)
            {
                services.Configure<GiteeStorageOptions>(configuration.Bind);
            }

            services.AddHttpClient<IStorageProvider, GiteeStorageProvider>()
                .ConfigurePrimaryHttpMessageHandler<NoProxyHttpClientHandler>();

            services.TryAddSingleton<IStorageProvider, GiteeStorageProvider>();
            return services;
        }
    }
}
