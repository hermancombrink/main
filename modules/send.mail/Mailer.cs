using log4net.logging;
using System;
using System.Collections.Generic;
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
            var auth = new NetworkCredential(Username, Password);
            mailContext.EnableSsl = EnableSSL;
            mailContext.Credentials = auth;
            mailContext.UseDefaultCredentials = false;
            this.IsBodyHtml = IsBodyHtml;
            _logger.Info("Done setting up mailer", TAG);

        }

        private void ConfigureMailer()
        {
            _logger.Info("Configuring mailer", TAG);
            try
            {
                System.Configuration.AppSettingsReader reader = new System.Configuration.AppSettingsReader();
                SMTPServer = reader.GetValue("smtpServer", typeof(string)).ToString();
                Port = (int)reader.GetValue("smtpPort", typeof(int));
                Username = reader.GetValue("smtpUserName", typeof(string)).ToString();
                Password = reader.GetValue("smtpPassword", typeof(string)).ToString();
                if (reader.GetValue("smtpSSL", typeof(bool)) != null)
                    EnableSSL = (bool)reader.GetValue("smtpSSL", typeof(bool));
                if (reader.GetValue("smtpHTML", typeof(bool)) != null)
                    IsBodyHtml = (bool)reader.GetValue("smtpHTML", typeof(bool));
            }
            catch (Exception ex)
            {
                throw _logger.GetSafeException(ex, "Unable to configure mailer. Please refer to logs", TAG);
            }
        }
        public async void SendMail(MailSetting setting)
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
                await mailContext.SendMailAsync(msg);
            }
            catch (Exception ex)
            {
                throw _logger.GetSafeException(ex, "Unable to send mail. Please refer to logs", TAG);
            }
        }

    }
}
