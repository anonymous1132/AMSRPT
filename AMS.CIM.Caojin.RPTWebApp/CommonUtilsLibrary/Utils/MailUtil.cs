using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AegisImplicitMail;
using CommonUtilsLibrary.Models;

namespace CommonUtilsLibrary.Utils
{
    public class MailUtil
    {
        public static void SendMail(MailUtilParaModel para,MailMessageModel message)
        {
            //Generate Message
            var mailMessage = new MimeMailMessage();
            mailMessage.From= new MimeMailAddress(para.MailAddress);
            //para.ReciverAddressList.ForEach(f=>mailMessage.To.Add(f));
            mailMessage.To.Add(para.ReciverAddresses);
            mailMessage.Subject = message.Subject;
            mailMessage.Body = message.Text;

            //Create Smtp Client
            var mailer = new MimeMailer(para.ServerHost, para.ServerPort);
            mailer.User = para.LogonUserName;
            mailer.Password = para.Password;
            mailer.SslType =(SslMode)para.SSLType ;
            mailer.AuthenticationMode = AuthenticationType.Base64;
            mailer.SendMailAsync(mailMessage);
        }


    }
}
