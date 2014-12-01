using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Common
{
    public class SendEmailOperation
    {
        public bool SendEmail(string email, string companyName, string phone, string startTime, string endTime, string user, string pwd, string cmd)
        {
            string sendAdress = ConfigurationManager.ConnectionStrings["sendAdress"].ToString();
            string sendEmailPwd = ConfigurationManager.ConnectionStrings["sendEmailPwd"].ToString();
            string EmailSmtp = ConfigurationManager.ConnectionStrings["EmailSmtp"].ToString();
            string EmailText = String.Empty;
            string EmailTitle=String.Empty;
            if (cmd == "admin")
            {
                EmailText = "<strong>尊敬的" + companyName + "：您好！</strong><br />" + ConfigurationManager.ConnectionStrings["EmailTextStaff"].ToString();
                EmailText += "敬请在要求的时间内登录网址<a href=\"" + ConfigurationManager.ConnectionStrings["LoginAdressAdmin"] + "\">" + ConfigurationManager.ConnectionStrings["LoginAdressAdmin"] + "</a>查看调查状态。您的登录账号为：" + user + "；密码为：" + pwd + "。<br />" + ConfigurationManager.ConnectionStrings["CompanyName"];
                EmailTitle = ConfigurationManager.ConnectionStrings["EmailTitleAdmin"].ToString();
            }
            else
            {
                EmailText = "<strong>尊敬的" + companyName + "员工：您好！</strong><br />" + ConfigurationManager.ConnectionStrings["EmailTextStaff"].ToString();
                EmailText += "在填写过程中，如有不明确的问题，请致电咨询" + phone + "。本问卷的填写时间为" + startTime + "-" + endTime + "。敬请在要求的时间内登录网址<a href=\"" + ConfigurationManager.ConnectionStrings["LoginAdressStaff"] + "\">" + ConfigurationManager.ConnectionStrings["LoginAdressStaff"] + "</a>填写问卷。您的登录账号为：" + user + "；密码为：" + pwd + "。<br />" + ConfigurationManager.ConnectionStrings["CompanyName"];
                EmailTitle = ConfigurationManager.ConnectionStrings["EmailTitleStaff"].ToString();
            }

            
            MailMessage myMail = new MailMessage();
            try
            {
                myMail.From = new MailAddress(sendAdress);
                myMail.To.Add(new MailAddress(email));
                myMail.Subject = EmailTitle;
                myMail.SubjectEncoding = Encoding.UTF8;
                myMail.Body = EmailText;
                myMail.BodyEncoding = Encoding.UTF8;
                myMail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = EmailSmtp;
                smtp.Port = Convert.ToInt32(ConfigurationManager.ConnectionStrings["EmailPort"].ToString()); //Gmail的smtp端口
                smtp.Credentials = new NetworkCredential(sendAdress, sendEmailPwd);
                smtp.EnableSsl = true; //Gmail要求SSL连接
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network; //Gmail的发送方式是通过网络的方式，需要指定
                smtp.Send(myMail);
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
