using System.Web.Mvc;
using WeihanLi.Extensions;

namespace ActivityReservation.WechatAPI.Helper
{
    public class WechatResult : ContentResult
    {
        public WechatResult(string content)
        {
            Content = content;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ClearContent();
            context.HttpContext.Response.ContentType = "text/xml";
            context.HttpContext.Response.OutputStream.Write((Content ?? "").Replace("\r\n", "\n").ToByteArray());
            context.HttpContext.Response.End();
        }
    }
}
