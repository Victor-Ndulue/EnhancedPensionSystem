using EnhancedPensionSystem_Application.Helpers.DTOs.Requests;
using Microsoft.AspNetCore.Identity;

namespace EnhancedPensionSystem_Application.Services.Abstractions;

public interface IEmailService
{
    IdentityResult SendEmail(EmailParams emailDTO);
}
