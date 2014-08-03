using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace send.mail
{
    public interface ISendMail
    {
       void SendEmail(string recipient, string subject, string body, string cc, string bcc);
       void SendEmail(string recipient, string subject, string body, string cc);
       void SendEmail(string recipient, string subject, string body); 
    }
}
