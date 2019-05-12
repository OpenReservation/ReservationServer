using Microsoft.AspNetCore.Builder;
using WeihanLi.Common;

namespace ActivityReservation.Extensions
{
    public static class DependenceResloverExtension
    {
        public static IApplicationBuilder UseDependencyReslover(this IApplicationBuilder applicationBuilder)
        {
            DependencyResolver.SetDependencyResolver(applicationBuilder.ApplicationServices.GetService);
            return applicationBuilder;
        }
    }
}
