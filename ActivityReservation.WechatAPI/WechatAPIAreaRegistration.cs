using System.Web.Mvc;

namespace ActivityReservation.WechatAPI
{
    public class WechatAPIAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "WechatAPI";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "WechatAPI_default",
                "WechatAPI/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces:new []{ "ActivityReservation.WechatAPI.Controllers" }
            );
        }
    }
}