using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace send.mail
{
    public class MailSetting : send.mail.IMailSetting
    {
        public List<string> To { get; set; }
        public List<string> CC { get; set; }

        public List<string> BCC { get; set; }

        public string Content { get; set; }

        public string From { get; set; }

        public string Subject  { get; set; }

        public List<System.Net.Mail.Attachment> attachments  { get; set; }
        
        public string BuildTemplate<T>(string templateName, T context)
        {
            
            return "";
        }

       
    }
}
