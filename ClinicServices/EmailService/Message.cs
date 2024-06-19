using Microsoft.AspNetCore.Http;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicServices.EmailService
{
    public class Message
    {
        public List<MailboxAddress> To {  get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public IFormFileCollection? Attachments { get; set; }
        public Message(IEnumerable<EmailAddress> to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress(x.DisplayName, x.Email)));
            Subject = subject;
            Content = content;
        }
        public Message(IEnumerable<EmailAddress> to, string subject, string content, IFormFileCollection attachments)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress(x.DisplayName, x.Email)));
            Subject = subject;
            Content = content;
            Attachments = attachments;
        }

    }
}
