using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace CoreMvc.Api
{
    public class Mail
    {
        public static void SendGmail(string subject, string body, string mailTo, ConfigurationManager confg)
        {
            var fromAddress = new MailAddress(confg["AppSettings:GMail:Account"]);
            var toAddress = new MailAddress(mailTo);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, confg["AppSettings:GMail:Password"])
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}
