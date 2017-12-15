using System.Web.Mvc;
using System.Web.Routing;

namespace ActivityReservation
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //notice route
            routes.MapRoute("Notice", "Notice/{path}", new { controller = "Home", action = "NoticeDetails" },
                namespaces: new string[] { "ActivityReservation.Controllers" });

            //default route
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "ActivityReservation.Controllers" },
                constraints: new { id = @"\d*" }
            );
        }
    }
}