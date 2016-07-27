using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Common
{
    public class SecurityHelper
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="strPwd">原字符串</param>
        /// <returns>加密后字符串</returns>
        public static string GetMD5(string strPwd)
        {
            //MD5 对象创建的两种方式
            //MD5 md5 = MD5.Create();
            MD5 md5 = new MD5CryptoServiceProvider();
            //将输入的密码转换成字节数组
            byte[] bPwd = Encoding.UTF8.GetBytes(strPwd);    
            //计算指定字节数组的哈希值
            byte[] bMD5 = md5.ComputeHash(bPwd);
            //释放加密服务提供类的所有资源
            md5.Clear();   
            StringBuilder sbMD5Pwd = new StringBuilder();
            for (int i = 0; i < bMD5.Length; i++)
            {
                //将每个字节数据转换为2位的16进制的字符
                sbMD5Pwd.Append(bMD5[i].ToString("X2"));
            }
            return sbMD5Pwd.ToString();
        }

        /// <summary>
        /// use sha1 to encrypt string
        /// </summary>
        public static string SHA1_Encrypt(string Source_String)
        {
            byte[] StrRes = Encoding.UTF8.GetBytes(Source_String);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }

        /// <summary>
        /// SHA256 加密
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        public static string SHA256_Encrypt(string sourceString)
        {
            byte[] data = Encoding.UTF8.GetBytes(sourceString);
            SHA256 shaM = SHA256.Create();
            byte[] result = shaM.ComputeHash(data);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in result)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }

        /// <summary>
        /// SHA384 加密
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        public static string SHA384_Encrypt(string sourceString)
        {
            byte[] data = Encoding.UTF8.GetBytes(sourceString);
            SHA384 shaM = SHA384.Create();
            byte[] result = shaM.ComputeHash(data);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in result)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }

        /// <summary>
        /// SHA512_加密
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        public static string SHA512_Encrypt(string sourceString)
        {
            byte[] data = Encoding.UTF8.GetBytes(sourceString);
            SHA512 shaM = new SHA512Managed();
            byte[] result = shaM.ComputeHash(data);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in result)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }

        /// <summary>
        /// 多重加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MulEncrypt(string str)
        {
            return SHA384_Encrypt(GetMD5(str));
        }

        #region 1.0 使用 票据对象 将 用户数据 加密成字符串 +string EncryptUserInfo(string userInfo)
        /// <summary>
        /// 使用 票据对象 将 用户数据 加密成字符串
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public static string EncryptUserInfo(string userInfo)
        {
            //1.1 将用户数据 存入 票据对象
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, "哈哈", DateTime.Now, DateTime.Now, true, userInfo);
            //1.2 将票据对象 加密成字符串（可逆）
            string strData = FormsAuthentication.Encrypt(ticket);
            return strData;
        }
        #endregion

        #region 2.0 加密字符串 解密 +string DecryptUserInfo(string cryptograph)
        /// <summary>
        /// 加密字符串 解密
        /// </summary>
        /// <param name="cryptograph">加密字符串</param>
        /// <returns></returns>
        public static string DecryptUserInfo(string cryptograph)
        {
            //1.1 将 加密字符串 解密成 票据对象
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cryptograph);
            //1.2 将票据里的 用户数据 返回
            return ticket.UserData;
        }
        #endregion

        /// <summary>
        /// 生成验证码 字节数组
        /// </summary>
        /// <returns>验证码图片的字节数组</returns>
        public static byte[] GenerateValidCode()
        {
            // 产生6位随机字符
            string strValidCode = GetValidCode(6);
            //定义宽120像素，高40像素的数据定义的图像对象
            Bitmap image = new Bitmap(140, 40);
            //绘制图片                                                     
            Graphics g = Graphics.FromImage(image);
            try
            {
                //创建随机数生成器
                Random random = new Random();
                //清除图片背景色                                       
                g.Clear(Color.White);
                //
                Pen p = null;
                Pen p0 = new Pen(Color.Blue);

                Pen p1 = new Pen(Color.Brown);

                Pen p2 = new Pen(Color.Black);
                // 随机产生图片的背景噪线
                for (int i = 0; i < 30; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    //
                    switch (i % 3)
                    {
                        case 0:
                            p = p0;
                            break;
                        case 1:
                            p = p1;
                            break;
                        case 2:
                            p = p2;
                            break;
                    }
                    g.DrawLine(p, x1, y1, x2, y2);
                }
                //设置图片字体风格
                Font font = new System.Drawing.Font("微软雅黑", 20, (System.Drawing.FontStyle.Bold));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 3, true);//设置画笔类型
                //绘制随机字符
                g.DrawString(strValidCode, font, brush, 5, 2);
                //绘制图片的前景噪点
                g.DrawRectangle(new Pen(Color.LightBlue), 0, 0, image.Width - 1, image.Height - 1);

                //建立存储区为内存的流
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    //将图像对象储存为内存流        
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    return ms.ToArray();
                }
            }
            finally
            {
                //释放资源
                g.Dispose();
                image.Dispose();
            }
        }

        /// <summary>
        /// 生成 指定内容 的 验证码 字节数组
        /// </summary>
        /// <returns>验证码图片的字节数组</returns>
        public static byte[] GetValidCode(string content)
        {
            // 验证码字串
            string strValidCode = content;
            //定义宽120像素，高40像素的数据定义的图像对象
            Bitmap image = new Bitmap(140, 40);
            //绘制图片                                                     
            Graphics g = Graphics.FromImage(image);
            try
            {
                //创建随机数生成器
                Random random = new Random();
                //清除图片背景色                                       
                g.Clear(Color.White);
                //定义画笔
                Pen p = Pens.Silver;
                // 随机产生图片的背景噪线
                for (int i = 0; i < 90; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    //画线
                    g.DrawLine(p, x1, y1, x2, y2);
                }
                //设置图片字体风格
                Font font = new System.Drawing.Font("微软雅黑", 20, (System.Drawing.FontStyle.Bold));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 3, true);//设置画笔类型
                //绘制随机字符
                g.DrawString(strValidCode, font, brush, 5, 2);
                //绘制图片的前景噪点
                g.DrawRectangle(new Pen(Color.LightBlue), 0, 0, image.Width - 1, image.Height - 1);

                //建立存储区为内存的流
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    //将图像对象储存为内存流        
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    return ms.ToArray();
                }
            }
            finally
            {
                //释放资源
                g.Dispose();
                image.Dispose();
            }
        }

        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="num">随机字符的个数</param>
        /// <returns>返回随机产生的字符串</returns>
        private static string GetValidCode(int num)
        {
            //定义一个允许的字符组成的字符串
            string strRandomCode = "ABCD1EF2GH3IJ4KL5MN6P7QR8ST9UVWXYZ";                                //定义要随机抽取的字符串
            //char[] chaStr = strRandomCode.ToCharArray();                                                //第二种方法：将定义的字符串转成字符数组
            StringBuilder sbValidCode = new StringBuilder();                                            //定义StringBuilder对象用于存放验证码
            //随机数生成器，用于随机产生验证码中字符
            Random rnd = new Random();                                                                   //随机函数，随机抽取字符
            for (int i = 0; i < num; i++)
            {
                //随机获取一个字符
                char a = strRandomCode[rnd.Next(0, strRandomCode.Length)];
                //拼接字符
                sbValidCode.Append(a);
            }
            return sbValidCode.ToString();
        }

        #region 直接生成验证码，并输出到浏览器

        /// <summary>
        /// 生成验证码，并直接输出到浏览器
        /// </summary>
        /// <param name="context">HTTPContextBase HTTP上下文对象</param>
        /// 通过HTTPContextBase对象将字符串保存到Session中，以便需要时进行验证，并将验证码输出到HTTPContextBase对象的Content属性中，输出到浏览器
        public static void GenerateValidCode(HttpContextBase context)
        {
            // 产生5位随机字符
            string strValidCode = GetValidCode(5);

            //如果要使用context.Session，需要添加命名空间System.Web.SessionState   using System.Web.SessionState;  ，然后再继承IRequiresSessionState接口
            //将字符串保存到Session中，以便需要时进行验证
            context.Session["ValidCode"] = strValidCode;
            //定义宽120像素，高30像素的数据定义的图像对象
            Bitmap image = new Bitmap(120, 40);
            //绘制图片                                                     
            Graphics g = Graphics.FromImage(image);
            try
            {
                //创建随机数生成器
                Random random = new Random();
                //清除图片背景色                                       
                g.Clear(Color.White);
                // 随机产生图片的背景噪线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                //设置图片字体风格
                Font font = new System.Drawing.Font("微软雅黑", 20, (System.Drawing.FontStyle.Bold));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 3, true);//设置画笔类型
                //绘制随机字符
                g.DrawString(strValidCode, font, brush, 5, 2);
                //绘制图片的前景噪点
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //建立存储区为内存的流
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                //将图像对象储存为内存流        
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                //清除当前缓冲区流中的所有内容      
                context.Response.ClearContent();
                //设置输出流的MIME类型
                context.Response.ContentType = "image/png";
                //将内存流写入到输出流                         
                context.Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                //释放资源
                g.Dispose();
                image.Dispose();
            }
        }

        /// <summary>
        /// 验证验证码是否正确
        /// </summary>
        /// <param name="code">用户输入的验证码</param>
        /// <param name="context">HttpContextBase对象</param>
        /// <returns>是否正确,<code>true</code>用户输入的验证码正确，<code>false</code>用户输入的验证码不正确</returns>
        public static bool ValidCode(string code, HttpContextBase context)
        {
            bool isValid = false;
            if (code.ToUpper() == context.Session["ValidCode"].ToString().ToUpper())
            {
                isValid = true;
            }
            return isValid;
        }

        #endregion
    }
}
