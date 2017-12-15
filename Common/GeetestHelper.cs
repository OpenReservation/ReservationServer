using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using WeihanLi.Common.Helpers;

namespace Common
{
    /// <summary>
    /// GreatestHelper 极验帮助类
    /// </summary>
    public class GeetestHelper
    {
        private string userID, captchaID, privateKey, responseStr;
        private GeetestResponseModel response;

        public GeetestResponseModel Response
        {
            get { return response; }

            set { response = value; }
        }

        /// <summary>
        /// GreatestHelper 构造函数
        /// </summary>
        /// <param name="publicKey">极验验证公钥</param>
        /// <param name="privateKey">极验验证私钥</param>
        public GeetestHelper(string publicKey, string privateKey)
        {
            this.privateKey = privateKey;
            captchaID = publicKey;
            response = new GeetestResponseModel()
            {
                gt = publicKey
            };
        }

        /// <summary>
        /// 验证初始化预处理
        /// </summary>
        /// <returns>初始化结果</returns>
        public Byte preProcess(string userID)
        {
            if (this.captchaID != null)
            {
                this.userID = userID;
                var challenge = this.registerChallenge();
                if (challenge.Length == 32)
                {
                    getSuccessPreProcessRes(challenge);
                    return 1;
                }
                else
                {
                    getFailPreProcessRes();
                }
            }
            return 0;
        }

        public string getResponseStr()
        {
            return responseStr;
        }

        /// <summary>
        /// 预处理失败后的返回格式串
        /// </summary>
        private void getFailPreProcessRes()
        {
            var rand1 = new Random().Next(100, 1000);
            var rand2 = new Random().Next(100, 1000);
            var md5Str1 = Encode(rand1.ToString() + "");
            var md5Str2 = Encode(rand2.ToString() + "");
            var challenge = md5Str1 + md5Str2.Substring(0, 2);
            response.success = 0;
            response.challenge = challenge;
        }

        /// <summary>
        /// 预处理成功后的标准串
        /// </summary>
        private void getSuccessPreProcessRes(string challenge)
        {
            challenge = Encode(challenge + this.privateKey);
            response.success = 1;
            response.challenge = challenge;
        }

        /// <summary>
        /// failback模式的验证方式
        /// </summary>
        /// <param name="challenge">failback模式下用于与validate一起解码答案， 判断验证是否正确</param>
        /// <param name="validate">failback模式下用于与challenge一起解码答案， 判断验证是否正确</param>
        /// <param name="seccode">failback模式下，其实是个没用的参数</param>
        /// <returns>验证结果</returns>
        public int failbackValidateRequest(string challenge, string validate, string seccode)
        {
            if (!this.requestIsLegal(challenge, validate, seccode))
            {
                return GeetestConsts.FailResult;
            }
            var validateStr = validate.Split('_');
            var encodeAns = validateStr[0];
            var encodeFullBgImgIndex = validateStr[1];
            var encodeImgGrpIndex = validateStr[2];
            var decodeAns = DecodeResponse(challenge, encodeAns);
            var decodeFullBgImgIndex = DecodeResponse(challenge, encodeFullBgImgIndex);
            var decodeImgGrpIndex = DecodeResponse(challenge, encodeImgGrpIndex);
            var validateResult = validateFailImage(decodeAns, decodeFullBgImgIndex, decodeImgGrpIndex);
            return validateResult;
        }

        private int validateFailImage(int ans, int full_bg_index, int img_grp_index)
        {
            const int thread = 3;
            var full_bg_name = this.Encode(full_bg_index + "").Substring(0, 10);
            var bg_name = Encode(img_grp_index + "").Substring(10, 10);
            var answer_decode = "";
            for (var i = 0; i < 9; i++)
            {
                if (i % 2 == 0) answer_decode += full_bg_name.ElementAt(i);
                else if (i % 2 == 1) answer_decode += bg_name.ElementAt(i);
            }
            var x_decode = answer_decode.Substring(4);
            var x_int = Convert.ToInt32(x_decode, 16);
            var result = x_int % 200;
            if (result < 40) result = 40;
            if (Math.Abs(ans - result) < thread)
            {
                return GeetestConsts.SuccessResult;
            }
            return GeetestConsts.FailResult;
        }

        private bool requestIsLegal(string challenge, string validate, string seccode)
        {
            if (String.IsNullOrEmpty(challenge) || String.IsNullOrEmpty(validate) || String.IsNullOrEmpty(seccode))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 向gt-server进行二次验证
        /// </summary>
        /// <param name="challenge">本次验证会话的唯一标识</param>
        /// <param name="validate">拖动完成后server端返回的验证结果标识字符串</param>
        /// <param name="seccode">验证结果的校验码，如果gt-server返回的不与这个值相等则表明验证失败</param>
        /// <returns>二次验证结果</returns>
        public int enhencedValidateRequest(string challenge, string validate, string seccode, string userID)
        {
            if (!this.requestIsLegal(challenge, validate, seccode))
            {
                return GeetestConsts.FailResult;
            }
            if (validate.Length > 0 && checkResultByPrivate(challenge, validate))
            {
                var query = "seccode=" + seccode + "&user_id=" + userID + "&sdk=csharp_" + GeetestConsts.Version;
                var response = "";
                try
                {
                    response = postValidate(query);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                if (response.Equals(Encode(seccode)))
                {
                    return GeetestConsts.SuccessResult;
                }
            }
            return GeetestConsts.FailResult;
        }

        private string registerChallenge()
        {
            var url = "";
            if (string.Empty.Equals(this.userID))
            {
                url = string.Format("{0}{1}?gt={2}", GeetestConsts.ApiUrl, GeetestConsts.RegisterUrl, this.captchaID);
            }
            else
            {
                url = string.Format("{0}{1}?gt={2}&user_id={3}", GeetestConsts.ApiUrl, GeetestConsts.RegisterUrl,
                    this.captchaID, this.userID);
            }
            var retstring = HttpHelper.HttpGetString(url);
            return retstring;
        }

        private bool checkResultByPrivate(string origin, string validate)
        {
            var encodeStr = Encode(privateKey + "geetest" + origin);
            return validate.Equals(encodeStr);
        }

        private string postValidate(string data)
        {
            var url = string.Format("{0}{1}", GeetestConsts.ApiUrl, GeetestConsts.ValidateUrl);
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Encoding.UTF8.GetByteCount(data);
            // 发送数据
            var myRequestStream = request.GetRequestStream();
            var requestBytes = Encoding.ASCII.GetBytes(data);
            myRequestStream.Write(requestBytes, 0, requestBytes.Length);
            myRequestStream.Close();
            var res = (HttpWebResponse)request.GetResponse();
            // 读取返回信息
            using (var myResponseStream = res.GetResponseStream())
            {
                if (myResponseStream != null)
                {
                    using (var myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8))
                    {
                        return myStreamReader.ReadToEnd();
                    }
                }
                return "";
            }
        }

        private int DecodeRandBase(string challenge)
        {
            var baseStr = challenge.Substring(32, 2);
            var tempList = new List<int>();
            for (var i = 0; i < baseStr.Length; i++)
            {
                var tempAscii = (int)baseStr[i];
                tempList.Add((tempAscii > 57)
                    ? (tempAscii - 87)
                    : (tempAscii - 48));
            }
            var result = tempList.ElementAt(0) * 36 + tempList.ElementAt(1);
            return result;
        }

        private int DecodeResponse(string challenge, string str)
        {
            if (str.Length > 100) return 0;
            var shuzi = new int[] { 1, 2, 5, 10, 50 };
            var chongfu = "";
            var key = new Hashtable();
            var count = 0;
            for (var i = 0; i < challenge.Length; i++)
            {
                var item = challenge.ElementAt(i) + "";
                if (!chongfu.Contains(item))
                {
                    var value = shuzi[count % 5];
                    chongfu += item;
                    count++;
                    key.Add(item, value);
                }
            }
            var res = 0;
            for (var i = 0; i < str.Length; i++) res += (int)key[str[i] + ""];
            res = res - this.DecodeRandBase(challenge);
            return res;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="plainText">普通文本</param>
        /// <returns></returns>
        private string Encode(string plainText)
        {
            var t2 = BitConverter.ToString(HashHelper.GetHashedBytes(HashType.MD5, plainText));
            t2 = t2.Replace("-", "");
            t2 = t2.ToLower();
            return t2;
        }
    }

    /// <summary>
    /// 极验响应model
    /// </summary>
    public class GeetestResponseModel
    {
        public int success { get; set; }

        public string gt { get; set; }

        public string challenge { get; set; }
    }

    /// <summary>
    /// GeetestLib 极验验证C# Consts
    /// </summary>
    public class GeetestConsts
    {
        /// <summary>
        /// SDK版本号
        /// </summary>
        public const string Version = "3.2.0";

        /// <summary>
        /// SDK开发语言
        /// </summary>
        public const string SdkLang = "csharp";

        /// <summary>
        /// 极验验证API URL
        /// </summary>
        public const string ApiUrl = "http://api.geetest.com";

        /// <summary>
        /// register url
        /// </summary>
        public const string RegisterUrl = "/register.php";

        /// <summary>
        /// validate url
        /// </summary>
        public const string ValidateUrl = "/validate.php";

        /// <summary>
        /// 极验验证API服务状态Session Key
        /// </summary>
        public const string GtServerStatusSessionKey = "gt_server_status";

        /// <summary>
        /// 极验验证二次验证表单数据 Chllenge
        /// </summary>
        public const string fnGeetestChallenge = "geetest_challenge";

        /// <summary>
        /// 极验验证二次验证表单数据 Validate
        /// </summary>
        public const string fnGeetestValidate = "geetest_validate";

        /// <summary>
        /// 极验验证二次验证表单数据 Seccode
        /// </summary>
        public const string fnGeetestSeccode = "geetest_seccode";

        /// <summary>
        /// 验证成功结果字符串
        /// </summary>
        public const int SuccessResult = 1;

        /// <summary>
        /// 证结失败验果字符串
        /// </summary>
        public const int FailResult = 0;

        /// <summary>
        /// 判定为机器人结果字符串
        /// </summary>
        public const string ForbiddenResult = "forbidden";

        /// <summary>
        /// public key
        /// </summary>
        public static readonly string publicKey = ConfigurationHelper.AppSetting("GeetestPublicKey");

        /// <summary>
        /// private key
        /// </summary>
        public static readonly string privateKey = ConfigurationHelper.AppSetting("GeetestPrivateKey");
    }
}