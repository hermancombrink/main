using System;
using System.Threading.Tasks;
namespace send.mail
{
    interface IMailer
    {
        void SendMail(MailSetting setting);

        Task SendMailAsync(MailSetting setting);
    }
}
