﻿using EnhancedPensionSystem_Application.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;
using System.Net;
using EnhancedPensionSystem_Application.Helpers.DTOs.Configs;
using EnhancedPensionSystem_Application.Helpers.DTOs.Requests;
using Microsoft.Extensions.Options;

namespace EnhancedPensionSystem_Application.Services.Implementations;

public sealed class EmailService: IEmailService
{
    private readonly EmailConfig _emailConfig;

    public EmailService(IOptions<EmailConfig> emailConfig)
    {
        _emailConfig = emailConfig.Value;
    }
    public IdentityResult
        SendEmail(EmailParams emailDTO)
    {
        try
        {
            var toEmail = emailDTO.RecipientEmail;
            var subject = emailDTO.EmailSubject;
            var body = emailDTO.EmailBody;
            var smtpHost = _emailConfig.HOST;
            int smtpPort = _emailConfig.PORT;
            var userName = _emailConfig.USERNAME;
            var mailFrom = emailDTO.SenderEmail;
            var smtpPassword = _emailConfig.PASSWORD;
            bool enableSsl = _emailConfig.ENABLESSL;
            var senderName = emailDTO.SenderName;
            bool isHtml = emailDTO.IsHtml;

            using (var smtpClient = new SmtpClient(smtpHost, smtpPort))
            {
                smtpClient.EnableSsl = enableSsl;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(userName, smtpPassword);
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(mailFrom, senderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml,
                };
                mailMessage.To.Add(toEmail);
                smtpClient.Send(mailMessage);

                return IdentityResult.Success;
            }
        }
        catch (Exception ex)
        {
            var errorMsg = $"something happened trying to send email. Details: {ex.StackTrace}";
            var identityError = new IdentityError { Code = "500", Description = errorMsg };
            return IdentityResult.Failed(identityError);
        }
    }
}
