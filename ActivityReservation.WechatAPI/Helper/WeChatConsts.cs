using Microsoft.Extensions.Configuration;
using WeihanLi.Common;

namespace ActivityReservation.WechatAPI.Helper
{
    internal class WeChatConsts
    {
        /// <summary>
        /// 定义Token，与微信公共平台上的Token保持一致
        /// </summary>
        public static readonly string Token = DependencyResolver.Current.ResolveService<IConfiguration>()["MpWechat:Token"];

        /// <summary>
        /// AppId 要与 微信公共平台 上的 AppId 保持一致
        /// </summary>
        public static readonly string AppId = DependencyResolver.Current.ResolveService<IConfiguration>()["MpWechat:AppId"];

        /// <summary>
        /// EncodingAESKey
        /// </summary>
        public static readonly string AESKey = DependencyResolver.Current.ResolveService<IConfiguration>()["MpWechat:AESKey"];

        /// <summary>
        /// AppSecret
        /// </summary>
        public static readonly string AppSecret = DependencyResolver.Current.ResolveService<IConfiguration>()["MpWechat:AppSecret"];
    }
}
