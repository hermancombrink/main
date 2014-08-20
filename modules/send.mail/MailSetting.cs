using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace send.mail
{
    public class MailSetting 
    {
        public List<string> To = new List<string>();
        public List<string> CC = new List<string>();
        public List<string> BCC = new List<string>();

        public string Content { get; set; }

        public string From { get; set; }

        public string Subject  { get; set; }

        public List<System.Net.Mail.Attachment> attachments = new List<System.Net.Mail.Attachment>();
        
        public string BuildTemplate<T>(string templateName, T context)
        {
            
            return "";
        }

       
    }
}
