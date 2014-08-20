using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using log4net.logging;
using log4net.logging.Fakes;
using send.mail;

namespace MailerTests
{
    [TestClass]
    public class MailTests
    {
        ILogger logger = new FakeLogger();

        [TestMethod]
        public void TestConfigureMail()
        {
            Mailer mailer = new Mailer(logger);
        }

        [TestMethod]
        public  void TestSendMail()
        {
            Mailer mailer = new Mailer(logger);
            var settings = new MailSetting();
            settings.To.Add("herman.combrink@gmail.com");
            settings.Content = "Test Email";
            settings.Subject = "Test";
            mailer.SendMailAsync(settings).Wait();
        }

        [TestMethod]
        public void TestBuildTemplate()
        {
            Mailer mailer = new Mailer(logger);
        }

        [TestMethod]
        public void TestSendMailWithTemplate()
        {
            Mailer mailer = new Mailer(logger);
        }

        [TestMethod]
        public void TestSendWithAttachment()
        {
            Mailer mailer = new Mailer(logger);
        }
    }
}
