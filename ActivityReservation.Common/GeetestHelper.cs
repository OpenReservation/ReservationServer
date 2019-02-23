using System;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Options;
using WeihanLi.Common.Helpers;

namespace ActivityReservation.Common
{
    public class GeetestOptions
    {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }

    /// <summary>
    ///   GreatestHelper 极验帮助类
    /// </summary>
    public class GeetestHelper
    {
        private string _userId;
        private readonly string _captchaId;
        private readonly string _privateKey;

        /// <summary>
        ///   GreatestHelper 构造函数
        /// </summary>
        /// <param name="publicKey">极验验证公钥</param>
        /// <param name="privateKey">极验验证私钥</param>
        public GeetestHelper(IOptions<GeetestOptions> option)
        {
            _privateKey = option.Value.PrivateKey;
            _captchaId = option.Value.PublicKey;
            Response = new GeetestResponseModel
            {
                gt = option.Value.PublicKey
            };
        }

        public GeetestResponseModel Response { get; set; }

        public bool ValidateRequest(GeetestRequestModel request, string userId, byte gtServerStatus, Action onValidateSuccess = null)
        {
            var result = gtServerStatus == 1 ? EnhencedValidateRequest(request.challenge, request.validate, request.seccode, userId) : FailbackValidateRequest(request.challenge, request.validate, request.seccode);
            if (result == 1)
            {
                onValidateSuccess?.Invoke();
                return true;
            }
            return false;
        }

        /// <summary>
        ///   验证初始化预处理
        /// </summary>
        /// <returns>初始化结果</returns>
        public byte PreProcess(string userId)
        {
            if (_captchaId != null)
            {
                _userId = userId;
                var challenge = RegisterChallenge();
                if (challenge.Length == 32)
                {
                    GetSuccessPreProcessRes(challenge);
                    return 1;
                }

                GetFailPreProcessRes();
            }

            return 0;
        }

        /// <summary>
        ///   预处理失败后的返回格式串
        /// </summary>
        private void GetFailPreProcessRes()
        {
            var rand1 = new Random().Next(100, 1000);
            var rand2 = new Random().Next(100, 1000);
            var md5Str1 = Encode(rand1 + "");
            var md5Str2 = Encode(rand2 + "");
            var challenge = md5Str1 + md5Str2.Substring(0, 2);
            Response.success = 0;
            Response.challenge = challenge;
        }

        /// <summary>
        ///   预处理成功后的标准串
        /// </summary>
        private void GetSuccessPreProcessRes(string challenge)
        {
            challenge = Encode(challenge + _privateKey);
            Response.success = 1;
            Response.challenge = challenge;
        }

        /// <summary>
        ///   failback模式的验证方式
        /// </summary>
        /// <param name="challenge">failback模式下用于与validate一起解码答案， 判断验证是否正确</param>
        /// <param name="validate">failback模式下用于与challenge一起解码答案， 判断验证是否正确</param>
        /// <param name="seccode">failback模式下，其实是个没用的参数</param>
        /// <returns>验证结果</returns>
        public int FailbackValidateRequest(string challenge, string validate, string seccode)
        {
            if (!requestIsLegal(challenge, validate, seccode)) return GeetestConsts.FailResult;

            var validateStr = validate.Split('_');
            var encodeAns = validateStr[0];
            var encodeFullBgImgIndex = validateStr[1];
            var encodeImgGrpIndex = validateStr[2];
            var decodeAns = DecodeResponse(challenge, encodeAns);
            var decodeFullBgImgIndex = DecodeResponse(challenge, encodeFullBgImgIndex);
            var decodeImgGrpIndex = DecodeResponse(challenge, encodeImgGrpIndex);
            var validateResult = ValidateFailImage(decodeAns, decodeFullBgImgIndex, decodeImgGrpIndex);
            return validateResult;
        }

        private int ValidateFailImage(int ans, int fullBgIndex, int imgGrpIndex)
        {
            const int thread = 3;
            var full_bg_name = Encode(fullBgIndex + "").Substring(0, 10);
            var bg_name = Encode(imgGrpIndex + "").Substring(10, 10);
            var answer_decode = "";
            for (var i = 0; i < 9; i++)
                if (i % 2 == 0)
                    answer_decode += full_bg_name.ElementAt(i);
                else if (i % 2 == 1)
                    answer_decode += bg_name.ElementAt(i);

            var x_decode = answer_decode.Substring(4);
            var x_int = Convert.ToInt32(x_decode, 16);
            var result = x_int % 200;
            if (result < 40) result = 40;
            if (Math.Abs(ans - result) < thread) return GeetestConsts.SuccessResult;

            return GeetestConsts.FailResult;
        }

        private bool requestIsLegal(string challenge, string validate, string seccode)
        {
            if (string.IsNullOrEmpty(challenge) || string.IsNullOrEmpty(validate) || string.IsNullOrEmpty(seccode)
            ) return false;

            return true;
        }

        /// <summary>
        ///   向gt-server进行二次验证
        /// </summary>
        /// <param name="challenge">本次验证会话的唯一标识</param>
        /// <param name="validate">拖动完成后server端返回的验证结果标识字符串</param>
        /// <param name="seccode">验证结果的校验码，如果gt-server返回的不与这个值相等则表明验证失败</param>
        /// <param name="userId">userId</param>
        /// <returns>二次验证结果</returns>
        public int EnhencedValidateRequest(string challenge, string validate, string seccode, string userId)
        {
            if (!requestIsLegal(challenge, validate, seccode)) return GeetestConsts.FailResult;

            if (validate.Length > 0 && CheckResultByPrivate(challenge, validate))
            {
                var query = "seccode=" + seccode + "&user_id=" + userId + "&sdk=csharp_" + GeetestConsts.Version;
                var response = "";
                try
                {
                    response = postValidate(query);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                if (response.Equals(Encode(seccode))) return GeetestConsts.SuccessResult;
            }

            return GeetestConsts.FailResult;
        }

        private string RegisterChallenge()
          => HttpHelper.HttpGetString(string.Empty.Equals(_userId) ? $"{GeetestConsts.ApiUrl}{GeetestConsts.RegisterUrl}?gt={_captchaId}" : $"{GeetestConsts.ApiUrl}{GeetestConsts.RegisterUrl}?gt={_captchaId}&user_id={_userId}");

        private bool CheckResultByPrivate(string origin, string validate)
          => validate.Equals(Encode(_privateKey + "geetest" + origin));

        private string postValidate(string data)
        {
            var url = $"{GeetestConsts.ApiUrl}{GeetestConsts.ValidateUrl}";
            return HttpHelper.HttpPost(url, Encoding.ASCII.GetBytes(data), false);
        }

        private int DecodeRandBase(string challenge)
        {
            var tempList = challenge.Substring(32, 2).ToCharArray().Select(_ =>
            {
                var temp = (int)_;
                return temp > 57 ? temp - 87 : temp - 48;
            }).ToArray();

            return tempList[0] * 36 + tempList[1];
        }

        private int DecodeResponse(string challenge, string str)
        {
            if (str.Length > 100) return 0;
            var shuzi = new[] { 1, 2, 5, 10, 50 };
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
            res = res - DecodeRandBase(challenge);
            return res;
        }

        /// <summary>
        ///   加密
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
    ///   极验响应model
    /// </summary>
    public class GeetestResponseModel
    {
        public int success { get; set; }

        public string gt { get; set; }

        public string challenge { get; set; }
    }

    public class GeetestRequestModel
    {
        public string challenge { get; set; }

        public string validate { get; set; }

        public string seccode { get; set; }
    }

    /// <summary>
    ///   GeetestLib 极验验证C# Consts
    /// </summary>
    public class GeetestConsts
    {
        /// <summary>
        ///   SDK版本号
        /// </summary>
        public const string Version = "3.2.0";

        /// <summary>
        ///   SDK开发语言
        /// </summary>
        public const string SdkLang = "csharp";

        /// <summary>
        ///   极验验证API URL
        /// </summary>
        public const string ApiUrl = "https://api.geetest.com";

        /// <summary>
        ///   register url
        /// </summary>
        public const string RegisterUrl = "/register.php";

        /// <summary>
        ///   validate url
        /// </summary>
        public const string ValidateUrl = "/validate.php";

        /// <summary>
        ///   极验验证API服务状态Session Key
        /// </summary>
        public const string GtServerStatusSessionKey = "gt_server_status";

        /// <summary>
        ///   极验验证二次验证表单数据 Chllenge
        /// </summary>
        public const string FnGeetestChallenge = "geetest_challenge";

        /// <summary>
        ///   极验验证二次验证表单数据 Validate
        /// </summary>
        public const string FnGeetestValidate = "geetest_validate";

        /// <summary>
        ///   极验验证二次验证表单数据 Seccode
        /// </summary>
        public const string FnGeetestSeccode = "geetest_seccode";

        /// <summary>
        ///   极验存在Session中的UserId
        /// </summary>
        public const string GeetestUserId = "geetest_userId";

        /// <summary>
        ///   验证成功结果字符串
        /// </summary>
        public const int SuccessResult = 1;

        /// <summary>
        ///   证结失败验果字符串
        /// </summary>
        public const int FailResult = 0;

        /// <summary>
        ///   判定为机器人结果字符串
        /// </summary>
        public const string ForbiddenResult = "forbidden";
    }
}
