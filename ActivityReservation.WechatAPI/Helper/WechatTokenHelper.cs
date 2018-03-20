using System;
using ActivityReservation.WechatAPI.Entities;
using WeihanLi.Common.Helpers;
using WeihanLi.Extensions;
using WeihanLi.Redis;

namespace ActivityReservation.WechatAPI.Helper
{
    public static class WechatTokenHelper
    {
        /// <summary>
        /// GetAccessTokenUrlFormat
        /// 0:appid
        /// 1:secret
        /// </summary>
        private const string GetAccessTokenUrlFormat = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";

        /// <summary>
        /// 获取公共号 AccessToken
        /// </summary>
        /// <returns>AccessToken</returns>
        public static string GetAccessToken()
        {
            var token = RedisManager.CacheClient.GetOrSet("wechat_access_token", () => RetryHelper.TryInvoke(() => HttpHelper.HttpGetFor<AccessTokenEntity>(
                    GetAccessTokenUrlFormat.FormatWith(WeChatConsts.AppId, WeChatConsts.AppSecret)),
                result => result.AccessToken.IsNotNullOrWhiteSpace()),
                TimeSpan.FromSeconds(7120));
            return token?.AccessToken;
        }
    }
}
