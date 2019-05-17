using Microsoft.AspNetCore.Builder;
using WeihanLi.Common;

namespace ActivityReservation.Extensions
{
    public static class DependenceResolverExtension
    {
        public static IApplicationBuilder UseDependencyResolver(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Use(async (context, next) =>
            {
                DependencyResolver.SetDependencyResolver(context.RequestServices.GetService);
                await next();
            });
            return applicationBuilder;
        }
    }
}
