using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FluentMysql.Site.Mail
{
    class EmailSimples
    {
        protected string AppConfig(string parametro)
        {
            return ConfigurationManager.AppSettings[parametro].ToString();
        }

        public MailMessage mailMessage { get; set; }

        public NetworkCredential networkCredential { get; set; }

        public SmtpClient smtpClient { get; set; }

        public int Port { get; set; }

        public static void Enviar(string assunto, string mensagem, IList<string> destinatario)
        {
            EmailSimples emailSimples = new EmailSimples();
            emailSimples.mailMessage.Subject = assunto;
            emailSimples.mailMessage.Body = mensagem;
            foreach (string item in destinatario)
            {
                emailSimples.mailMessage.To.Add(item);
            }
            emailSimples.smtpClient.Credentials = emailSimples.networkCredential;
            emailSimples.smtpClient.Send(emailSimples.mailMessage);
            emailSimples.smtpClient.Dispose();
        }

        public void Enviar()
        {
            smtpClient.Credentials = networkCredential;
            smtpClient.Send(mailMessage);
            smtpClient.Dispose();
        }

        public EmailSimples()
        {
            int porta = 0;
            int.TryParse(AppConfig("mailPort"), out porta);

            mailMessage = new MailMessage();
            mailMessage.IsBodyHtml = true;
            mailMessage.Priority = MailPriority.Normal;
            mailMessage.From = new MailAddress(AppConfig("mailAddress"), AppConfig("mailDisplayName"));

            networkCredential = new NetworkCredential();
            networkCredential.UserName = AppConfig("mailUserName");
            networkCredential.Password = AppConfig("mailPassword");

            smtpClient = new SmtpClient();
            smtpClient.Host = AppConfig("mailHost");
            if (porta > 0)
                smtpClient.Port = porta;

        }
    }
}
