using log4net.logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace send.mail
{
    public class Mailer : send.mail.IMailer
    {
        private ILogger _logger;

        private string SMTPServer { get; set; }
        private int Port { get; set; }
        private string Username { get; set; }
        private string Password { get; set; }
        private bool EnableSSL { get; set; }
        private bool IsBodyHtml { get; set; }
        private System.Net.Mail.SmtpClient mailContext;
        private const string TAG = "Mailer";
        public Mailer(ILogger logger)
        {
            _logger = logger;
            _logger.Info("Setting up mailer", TAG);
            this.IsBodyHtml = true;
            this.EnableSSL = true;
            ConfigureMailer();
            mailContext = new SmtpClient(SMTPServer, Port);
            mailContext.EnableSsl = EnableSSL;
            
            mailContext.UseDefaultCredentials = false;
            mailContext.DeliveryMethod = SmtpDeliveryMethod.Network;
            this.IsBodyHtml = IsBodyHtml;
            _logger.Info("Done setting up mailer", TAG);

        }

        private void ConfigureMailer()
        {
            _logger.Info("Configuring mailer", TAG);
            try
            {
                SMTPServer = ConfigurationManager.AppSettings["smtpServer"];
                Port = Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);
                Username = ConfigurationManager.AppSettings["smtpUserName"];
                Password = ConfigurationManager.AppSettings["smtpPassword"];
                if (ConfigurationManager.AppSettings["smtpSSL"] != null)
                    EnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["smtpSSL"]);
                if (ConfigurationManager.AppSettings["smtpHTML"] != null)
                    IsBodyHtml = Convert.ToBoolean(ConfigurationManager.AppSettings["smtpHTML"]);
            }
            catch (Exception ex)
            {
                throw _logger.GetSafeException(ex, "Unable to configure mailer. Please refer to logs", TAG);
            }
        }
        public async Task SendMailAsync(MailSetting setting)
        {
            _logger.Info("Preparing to send mail", TAG);
            MailMessage msg = new MailMessage();
            if (setting.To != null)
                foreach (var item in setting.To)
                {
                    msg.To.Add(item);
                }
            if (setting.CC != null)
                foreach (var item in setting.CC)
                {
                    msg.CC.Add(item);
                }
            if (setting.BCC != null)
                foreach (var item in setting.BCC)
                {
                    msg.Bcc.Add(item);
                }

            if (!string.IsNullOrEmpty(setting.From))
                msg.From = new MailAddress(setting.From);
            else
                msg.From = new MailAddress(Username);
            msg.Body = setting.Content;
            msg.IsBodyHtml = IsBodyHtml;
            msg.Subject = setting.Subject;
            if (setting.attachments != null)
                foreach (var item in setting.attachments)
                {
                    msg.Attachments.Add(item);
                }
            try
            {
                 mailContext.Credentials = new NetworkCredential(Username, Password);
                await mailContext.SendMailAsync(msg);
            }
            catch (Exception ex)
            {
                throw _logger.GetSafeException(ex, "Unable to send mail. Please refer to logs", TAG);
            }
        }

        public void SendMail(MailSetting setting)
        {
            _logger.Info("Preparing to send mail", TAG);
            MailMessage msg = new MailMessage();
            if (setting.To != null)
                foreach (var item in setting.To)
                {
                    msg.To.Add(item);
                }
            if (setting.CC != null)
                foreach (var item in setting.CC)
                {
                    msg.CC.Add(item);
                }
            if (setting.BCC != null)
                foreach (var item in setting.BCC)
                {
                    msg.Bcc.Add(item);
                }

            if (!string.IsNullOrEmpty(setting.From))
                msg.From = new MailAddress(setting.From);
            else
                msg.From = new MailAddress(Username);
            msg.Body = setting.Content;
            msg.IsBodyHtml = IsBodyHtml;
            msg.Subject = setting.Subject;
            if (setting.attachments != null)
                foreach (var item in setting.attachments)
                {
                    msg.Attachments.Add(item);
                }
            try
            {
                mailContext.Credentials = new NetworkCredential(Username, Password);
                mailContext.Send(msg);
            }
            catch (Exception ex)
            {
                throw _logger.GetSafeException(ex, "Unable to send mail. Please refer to logs", TAG);
            }
        }

    }
}
