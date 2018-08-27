
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace Common.Lib.Common.Email
{
    /// <summary>
    /// Wrapper around MS's SmtpClient class that sets up the proper email
    /// settings for authentication against our domain.
    /// </summary>
    public static class Emailer
    {
        public static void SendGmailSupportMessage(MailAddressCollection toMailAddressCollection, string subject, string body)
        {
            SendGmailMessage(new MailAddress("support@qssolutions.net", "support"), toMailAddressCollection, "support@qss", subject, body);
        }

        public static void SendGmailSupportMessage(MailAddressCollection toMailAddressCollection, string subject, string body, IEnumerable<string> attachmentPaths)
        {
            SendGmailMessage(new MailAddress("support@qssolutions.net", "support"), toMailAddressCollection, "support@qss", subject, body, attachmentPaths);
        }
        public static void SendGmailMessage(MailAddress fromMailAddress, MailAddressCollection toMailAddressCollection, string fromPassword, string subject, string body)
        {
            SendMessage("smtp.gmail.com", 587, fromMailAddress, toMailAddressCollection, fromPassword, subject, body, null);
        }

        public static void SendGmailMessage(MailAddress fromMailAddress, MailAddressCollection toMailAddressCollection, string fromPassword, string subject, string body, IEnumerable<string> attachmentPaths)
        {
            SendMessage("smtp.gmail.com", 587, fromMailAddress, toMailAddressCollection, fromPassword, subject, body, attachmentPaths);
        }

        public static void SendMessage(string host, int port, MailAddress fromMailAddress, MailAddressCollection toMailAddressCollection, string fromPassword, string subject, string body, IEnumerable<string> attachmentPaths)
        {
            var smtp = new SmtpClient
            {
                Host = host,
                Port = port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromMailAddress.Address, fromPassword),
                Timeout = 20000
            };

            using (var message = new MailMessage{Subject = subject,Body = body})
            {
                foreach (var toMailAddress in toMailAddressCollection)
                {
                    message.To.Add(toMailAddress);
                }

                if (attachmentPaths != null)
                {
                    foreach (string attachment in attachmentPaths)
                    {
                        message.Attachments.Add(new Attachment(attachment));
                    }
                }

                message.From = fromMailAddress; 
                smtp.Send(message);
            }
        }
    }
}

