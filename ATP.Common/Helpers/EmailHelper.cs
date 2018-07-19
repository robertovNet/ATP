using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ATP.Common.Helpers
{
    public class EmailHelper
    {
        private const int SMTP_CLIENT_TIMEOUT = 20000;

        public static void Send(IEnumerable<string> emailRecipients, string subject, string bodyTemplate, params string[] templateParams)
        {
            try
            {
                using (var smtpClient = new SmtpClient())
                {
                    SetupClient(smtpClient);

                    using (var message = new MailMessage())
                    {
                        SetupMessage(message, emailRecipients, subject, bodyTemplate, templateParams);
                        smtpClient.Send(message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Hubo un error en el envío de e-mails a través de SmtpClient", ex);
            }
        }

        public static async Task SendAsync(IEnumerable<string> emailRecipients, string subject, string bodyTemplate, params string[] templateParams)
        {
            try
            {
                using (var smtpClient = new SmtpClient())
                {
                    SetupClient(smtpClient);

                    using (var message = new MailMessage())
                    {
                        SetupMessage(message, emailRecipients, subject, bodyTemplate, templateParams);
                        await smtpClient.SendMailAsync(message).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Hubo un error en el envío de e-mails a través de SmtpClient", ex);
            }
        }

        private static void SetupClient(SmtpClient client)
        {
            var userName = ConfigurationManager.AppSettings["smtpUserName"];
            var password = ConfigurationManager.AppSettings["smtpPassword"];

            client.Timeout = SMTP_CLIENT_TIMEOUT;
            client.Credentials = new NetworkCredential(userName, password);
        }

        private static void SetupMessage(MailMessage message, IEnumerable<string> recipients, string subject, string bodyTemplate, params string[] templateParams)
        {
            if (recipients == null || !recipients.Any(re => !string.IsNullOrWhiteSpace(re)))
                throw new ArgumentNullException("recipients", "La lista de correos de los destinatarios debe tener al menos un elemento.");

            var userName = ConfigurationManager.AppSettings["smtpUserName"];
            var displayName = ConfigurationManager.AppSettings["smtpDisplayName"];

            message.From = new MailAddress(userName, displayName);
            message.Subject = subject;
            message.IsBodyHtml = true;

            if (!string.IsNullOrWhiteSpace(bodyTemplate) && templateParams != null && templateParams.Any())
                message.Body = string.Format(bodyTemplate, templateParams);
            else if (!string.IsNullOrWhiteSpace(bodyTemplate))
                message.Body = bodyTemplate;

            recipients = recipients.Where(re => !string.IsNullOrWhiteSpace(re));

            if (recipients.Count() > 1)
                message.Bcc.Add(string.Join(",", recipients));
            else
                message.To.Add(recipients.First());
        }
    }
}
