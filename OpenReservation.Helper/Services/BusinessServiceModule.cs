using OpenReservation.Business;
using OpenReservation.Events;
using OpenReservation.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WeihanLi.Common.Event;
using WeihanLi.EntityFramework;
using WeihanLi.Extensions;

namespace OpenReservation.Services
{
    public class BusinessServiceModule : IServiceModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEFRepository();
            services.AddBLL();
            services.TryAddScoped<ReservationHelper>();
            services.TryAddSingleton<CaptchaVerifyHelper>();
            services.TryAddSingleton<OperLogHelper>();

            // register eventHandlers
            services.RegisterAssemblyTypes(t => !t.IsAbstract && t.IsClass && t.IsAssignableTo<IEventHandler>(), typeof(NoticeViewEventHandler).Assembly);
        }
    }
}
