using System;
using System.Collections.Generic;
namespace send.mail
{
    interface IMailSetting
    {
        System.Collections.Generic.List<string> BCC { get; set; }
        string BuildTemplate<T>(string templateName, T context);
        System.Collections.Generic.List<string> CC { get; set; }
        string Content { get; set; }
        string From { get; set; }
        string Subject { get; set; }
        List<System.Net.Mail.Attachment> attachments { get; set; }
        System.Collections.Generic.List<string> To { get; set; }
    }
}
