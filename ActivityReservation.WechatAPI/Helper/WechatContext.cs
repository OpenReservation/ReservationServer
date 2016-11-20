using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityReservation.WechatAPI.Helper
{
    abstract class WechatContext
    {
        protected string UserId { get; set; }

        protected abstract string GetResponse(string encryptMessage, Model.WeChatMsgRequestModel request);
    }
}
