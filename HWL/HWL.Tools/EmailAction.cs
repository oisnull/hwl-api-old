using System;
using System.Net.Mail;
using System.Text;

namespace HWL.Tools
{
    public class EmailAction
    {
        static readonly string systemEmail = "1577592799@qq.com";
        //static readonly string systemEmailPwd = "293051liy";
        static readonly string systemName = "hwl v1.0";
        static readonly string authCode = "pqifivnkxippfjff";

        public static int smtpPort = 25;
        public static string smtpHost = "smtp.qq.com";
        public static bool isSSL = true;

        /// <summary>
        /// 邮件发送
        /// </summary>
        /// <param name="title">邮件主题</param>
        /// <param name="content">发送的内容</param>
        /// <param name="receiveEmail">接收邮件</param>
        /// <returns></returns>
        public static bool SendEmail(string title, string content, string receiveEmail, out string errorInfo)
        {
            errorInfo = "";
            MailAddress from = new MailAddress(systemEmail, systemName);
            MailAddress to = new MailAddress(receiveEmail);

            MailMessage msg = new MailMessage(from, to);
            msg.BodyEncoding = Encoding.UTF8;
            msg.SubjectEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.Subject = title;
            msg.Body = content;
            msg.Priority = MailPriority.Normal;

            SmtpClient client = new SmtpClient(smtpHost);
            if (smtpPort != 0)
            {
                client.Port = smtpPort;
            }
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = isSSL;
            client.Credentials = new System.Net.NetworkCredential(systemEmail, authCode);

            try
            {
                client.Send(msg);
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                return false;
            }
            return true;
        }
    }
}
