using Api.Configs;
using Api.Models.Mail;
using DAL;
using DAL.Entities.Attaches;
using DAL.Entities.Users;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using Org.BouncyCastle.Asn1.Pkcs;
using RazorEngineCore;
using System.Runtime;
using System.Text;

namespace Common.Services
{
    public class EmailService
    {
        private readonly MailConfig _mailConfig;
        private readonly DataContext _dataContext;

        public EmailService(IOptions<MailConfig> mailConfig, DataContext dataContext)
        {
            _mailConfig = mailConfig.Value;
            _dataContext = dataContext;
        }

        public async Task<bool> SendEmailAsync(MailModel model, CancellationToken ct = default)
        {
            try
            {
                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress(_mailConfig.DisplayName, model.From ?? _mailConfig.From));
                emailMessage.Sender = new MailboxAddress(model.DisplayName ?? _mailConfig.DisplayName, model.From ?? _mailConfig.From);

                // Receiver
                foreach (string mailAddress in model.To)
                    emailMessage.To.Add(MailboxAddress.Parse(mailAddress));

                // Set Reply to if specified in emailMessage data
                if (!string.IsNullOrEmpty(model.ReplyTo))
                    emailMessage.ReplyTo.Add(new MailboxAddress(model.ReplyToName, model.ReplyTo));

                // BCC
                // Check if a BCC was supplied in the request
                if (model.Bcc != null)
                {
                    // Get only addresses where value is not null or with whitespace. x = value of address
                    foreach (string mailAddress in model.Bcc.Where(x => !string.IsNullOrWhiteSpace(x)))
                        emailMessage.Bcc.Add(MailboxAddress.Parse(mailAddress.Trim()));
                }

                // CC
                // Check if a CC address was supplied in the request
                if (model.Cc != null)
                {
                    foreach (string mailAddress in model.Cc.Where(x => !string.IsNullOrWhiteSpace(x)))
                        emailMessage.Cc.Add(MailboxAddress.Parse(mailAddress.Trim()));
                }

                #region Content

                // Add Content to Mime Message
                var body = new BodyBuilder();
                emailMessage.Subject = model.Subject;
                body.HtmlBody = model.Body;
                emailMessage.Body = body.ToMessageBody();

                // Check if we got any attachments and add the to the builder for our message
                if (model.Attachments != null)
                {
                    byte[] attachmentFileByteArray;

                    foreach (IFormFile attachment in model.Attachments)
                    {
                        // Check if length of the file in bytes is larger than 0
                        if (attachment.Length > 0)
                        {
                            // Create a new memory stream and attach attachment to mail body
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                // Copy the attachment to the stream
                                attachment.CopyTo(memoryStream);
                                attachmentFileByteArray = memoryStream.ToArray();
                            }
                            // Add the attachment from the byte array
                            body.Attachments.Add(attachment.FileName, attachmentFileByteArray, ContentType.Parse(attachment.ContentType));
                        }
                    }
                }

                #endregion

                #region Send emailMessage

                using var smtp = new SmtpClient();

                if (_mailConfig.UseSSL)
                {
                    await smtp.ConnectAsync(_mailConfig.Host, _mailConfig.Port, SecureSocketOptions.SslOnConnect, ct);
                }
                else if (_mailConfig.UseStartTls)
                {
                    await smtp.ConnectAsync(_mailConfig.Host, _mailConfig.Port, SecureSocketOptions.StartTls, ct);
                }
                await smtp.AuthenticateAsync(_mailConfig.UserName, _mailConfig.Password, ct);
                await smtp.SendAsync(emailMessage, ct);
                await smtp.DisconnectAsync(true, ct);

                #endregion

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GetEmailTemplate<T>(string emailTemplate, T emailTemplateModel)
        {
            string mailTemplate = LoadTemplate(emailTemplate);

            IRazorEngine razorEngine = new RazorEngine();
            IRazorEngineCompiledTemplate modifiedMailTemplate = razorEngine.Compile(mailTemplate);

            return modifiedMailTemplate.Run(emailTemplateModel);
        }

        public string LoadTemplate(string emailTemplate)
        {
            //string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string baseDir = "C:\\Users\\angry\\source\\repos\\DigitalDesignStudy\\Api"; //Path.GetTempPath();
            string templateDir = Path.Combine(baseDir, "attaches/MailTemp"); //C: \Users\angry\source\repos\DigitalDesignStudy\Api\attaches\MailTemp\welcome.cshtml
            string templatePath = Path.Combine(templateDir, $"{emailTemplate}.cshtml");

            using FileStream fileStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using StreamReader streamReader = new StreamReader(fileStream, Encoding.Default);

            string mailTemplate = streamReader.ReadToEnd();
            streamReader.Close();

            return mailTemplate;
        }
    }
}
