using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class EmailServiceHelper
    {
        private static SmtpClient mailServiceProvider = null;

        public void InitEmailService(string mailName,string mailPass)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = new NetworkCredential(mailName,mailPass);
            string host = "smtp."+mailName.Split('@')[1];
            smtp.Host = host;
            mailServiceProvider = smtp;
        }

        public void SendMail(string subject,string body,string mailTo,bool isBodyHtml=true)
        {
            if (mailServiceProvider == null)
            {
                System.Diagnostics.Debug.WriteLine("邮箱服务尚未初始化");
                return;
            }
            MailMessage mailObj = new MailMessage();
            mailObj.From = new MailAddress("hengyangxieyue@sina.com"); //发送人邮箱地址
            string[] mailToAddresses = mailTo.Split(',');
            foreach (string item in mailToAddresses)
            {
                mailObj.To.Add(item);
            }
            mailObj.Subject = subject;    //主题
            mailObj.Body = body;    //正文
            mailObj.IsBodyHtml = isBodyHtml;            
            //send mail
            mailServiceProvider.Send(mailObj);
        }
    }
}