using log4net.logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace send.mail
{
    public class SendSMTPMail : ISendMail
    {
        /* *
      <to value="herman.combrink@gmail.com" />
      <from value="fila101@gmail.com" />
      <subject value="ginger-aalwyn.co.za ERROR!" />
      <smtpHost value="smtp.gmail.com" />
      <port value="587" />
      <authentication value="Basic" />
      <username value="fila101@gmail.com" />
      <password value="angora101" />
      <EnableSsl value="true" />
         * */
        private ILogger _logger;
        private const string TAG = "SendMail";


        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }


        public SendSMTPMail(ILogger logger)
        {
            _logger = logger;
            ConfigMailer();
        }

        private void ConfigMailer()
        {
            try
            {
                var reader = new System.Configuration.AppSettingsReader();

                Host = (string)reader.GetValue("host", typeof(string));
                Port = (int)reader.GetValue("port", typeof(int));
                EnableSSL = (bool)reader.GetValue("enablessl", typeof(bool));
                Username = (string)reader.GetValue("username", typeof(string));
                Password = (string)reader.GetValue("password", typeof(string));

            }
            catch (Exception ex)
            {
                throw _logger.GetSafeException(ex, "Failed to configure mailer. Check configuration for mail settings", TAG);
            }
        }

        public void SendEmail(string recipient, string subject, string body, string cc, string bcc)
        {
            throw new NotImplementedException();
        }

        public void SendEmail(string recipient, string subject, string body, string cc)
        {
            throw new NotImplementedException();
        }

        public void SendEmail(string recipient, string subject, string body)
        {
            throw new NotImplementedException();
        }
    }
}
