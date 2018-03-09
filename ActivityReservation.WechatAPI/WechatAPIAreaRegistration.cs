using System.Web.Mvc;

namespace ActivityReservation.WechatAPI
{
    public class WechatAPIAreaRegistration : AreaRegistration
    {
        public override string AreaName => "Wechat";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Wechat_default",
                "Wechat/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "ActivityReservation.WechatAPI.Controllers" }
            );
        }
    }
}
