using System;
namespace send.mail
{
    interface IMailer
    {
        void SendMail(MailSetting setting);
    }
}
