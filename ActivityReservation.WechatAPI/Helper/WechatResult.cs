using Microsoft.AspNetCore.Mvc;
using WeihanLi.Extensions;

namespace ActivityReservation.WechatAPI.Helper
{
    internal class WechatResult : ContentResult
    {
        public WechatResult(string content)
        {
            Content = content;
        }

        public override void ExecuteResult(ActionContext context)
        {
            context.HttpContext.Response.ContentType = "text/xml;charset=utf-8";
            context.HttpContext.Response.Body.Write((Content ?? "").Replace("\r\n", "\n").ToByteArray());
        }
    }
}
