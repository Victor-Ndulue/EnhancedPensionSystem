namespace EnhancedPensionSystem_Application.Helpers.DTOs.Requests;

public record EmailParams
(
    string SenderEmail,
    string EmailSubject,
    string EmailBody,
    string RecipientEmail,
    string SenderName,
    bool IsHtml
);

