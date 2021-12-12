﻿using System;
using System.Linq;
using OpenReservation.WechatAPI.Helper;
using OpenReservation.WechatAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WeihanLi.Common.Helpers;

namespace OpenReservation.WechatAPI.Filters;

public class WechatRequestValidAttribute : Attribute, IAuthorizationFilter
{
    private static bool CheckSignature(WechatMsgRequestModel model, string token)
    {
        //获取请求来的参数
        var signature = model.Signature;
        var timestamp = model.Timestamp;
        var nonce = model.Nonce;
        //创建数组，将 Token, timestamp, nonce 三个参数加入数组
        string[] array = { token, timestamp, nonce };
        //进行排序
        Array.Sort(array);
        //拼接为一个字符串
        var tempStr = string.Join("", array);
        //对字符串进行 SHA1加密
        tempStr = SecurityHelper.SHA1(tempStr);
        //判断signature 是否正确
        return tempStr.Equals(signature?.ToUpper());
    }

    public void OnAuthorization(AuthorizationFilterContext filterContext)
    {
        var model = new WechatMsgRequestModel
        {
            Nonce = filterContext.HttpContext.Request.Query["nonce"].FirstOrDefault(),
            Signature = filterContext.HttpContext.Request.Query["signature"].FirstOrDefault(),
            Timestamp = filterContext.HttpContext.Request.Query["timestamp"].FirstOrDefault(),
            Msg_Signature = filterContext.HttpContext.Request.Query["msg_signature"].FirstOrDefault()
        };
        //验证
        var token = filterContext.HttpContext.Request.Path.Value.ToLower().Contains("/wechatapp/")
            ? WxAppConsts.Token
            : MpWeChatConsts.Token;
        if (string.IsNullOrEmpty(token))
        {
            token = "Reservation";
        }
        if (!CheckSignature(model, token))
        {
            filterContext.HttpContext.RequestServices.GetRequiredService<ILogger<WechatRequestValidAttribute>>()
                .LogWarning("微信请求签名验证不通过, signature: {Signature}", model.Signature);

            filterContext.Result = new ContentResult
            {
                Content = "微信请求验证失败",
                StatusCode = 401,
                ContentType = "text/plain;charset=utf-8",
            };
        }
    }
}