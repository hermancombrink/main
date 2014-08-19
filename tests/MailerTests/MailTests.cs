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
    }
}
