using EnhancedPensionSystem_Application.Helpers.DTOs.Requests;
using EnhancedPensionSystem_Application.Helpers.DTOs.Responses;
using EnhancedPensionSystem_Application.Helpers.ObjectFormatter;
using EnhancedPensionSystem_Application.Services.Abstractions;
using EnhancedPensionSystem_Application.UnitOfWork.Abstraction;
using EnhancedPensionSystem_Domain.Enums;
using EnhancedPensionSystem_Domain.Models;
using EnhancedPensionSystem_Infrastructure.Repository.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EnhancedPensionSystem_Application.Services.Implementations;

public sealed class EmployerService : IEmployerService
{
    private readonly IGenericRepository<Employer> _employerRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly string? senderEmail = Environment.GetEnvironmentVariable("senderEmail");

    public EmployerService(IGenericRepository<Employer> employerRepository, UserManager<AppUser> userManager, IServiceManager serviceManager)
    {
        _employerRepository = employerRepository;
        _userManager = userManager;
        _emailService = serviceManager.EmailService;
    }

    public async Task<bool>
        ConfirmEmployerExistsAsync(string? employerId)
    {
        return await _employerRepository.ExistsByConditionAsync(e => e.Id == employerId);
    }

    public async Task<string?>
        GetEmployerNameByIdAsync(string? employerId)
    {
        string? employerName = await _employerRepository.GetNonDeletedByCondition(employer => employer.Id == employerId)
            .Select(employer => employer.CompanyName).FirstOrDefaultAsync();
        return employerName;
    }

    public async Task<Dictionary<string, string>>
        GetEmployersNameDictionaryAsync()
    {
        var employersNameDictionary = await _employerRepository.GetAllNonDeleted()
            .ToDictionaryAsync(employer => employer.Id, employer => employer.CompanyName!);
        return employersNameDictionary;
    }

    public async Task<StandardResponse<IEnumerable<EmployerResponse>>>
        GetAllEmployersAsync()
    {
        var employers = await _employerRepository.GetAllNonDeleted()
            .Select(e => new EmployerResponse
            (
                e.Id, e.CompanyName!, e.RegistrationNumber!,
                e.IsActive, e.Email!, e.PhoneNumber!
            )).AsNoTracking()
            .ToListAsync();

        return StandardResponse<IEnumerable<EmployerResponse>>.Success(employers);
    }

    public async Task<StandardResponse<EmployerResponse>>
        GetEmployerByIdAsync(string employerId)
    {
        var employerResponse = await _employerRepository.GetNonDeletedByCondition(e => e.Id == employerId)
            .Select(employer => new EmployerResponse
            (
                employer.Id, employer.CompanyName!, employer.RegistrationNumber!,
                employer.IsActive, employer.Email!, employer.PhoneNumber!
            )).AsNoTracking()
            .FirstOrDefaultAsync();

        return StandardResponse<EmployerResponse>.Success(employerResponse);
    }

    public async Task<StandardResponse<string>>
        RegisterEmployerAsync(CreateEmployerParams createEmployerParams)
    {
        // Validate Company Name & Registration Number
        if (string.IsNullOrWhiteSpace(createEmployerParams.companyName))
        {
            string? errorMsg = "Company name is required.";
            return StandardResponse<string>.Failed(errorMsg);
        }

        if (string.IsNullOrWhiteSpace(createEmployerParams.registrationNumber))
        {
            string errorMsg = "Registration number is required.";
            return StandardResponse<string>.Failed(errorMsg);
        }

        // Check if employer already exists
        bool employerExists = await _employerRepository.ExistsByConditionAsync(
            e => e.RegistrationNumber == createEmployerParams.registrationNumber);

        if (employerExists)
        {
            string errorMSg = "Employer with this registration number already exists.";
            return StandardResponse<string>.Failed(errorMSg);
        }

        var employer = new Employer
        {
            CompanyName = createEmployerParams.companyName,
            RegistrationNumber = createEmployerParams.registrationNumber,
            IsActive = true,
            UserName = createEmployerParams.email,
            Email = createEmployerParams.email,
            PhoneNumber = createEmployerParams.phoneNumber,
            AppUserType = AppUserType.Employer
        };

        string defaultPassword = GenerateRandomPassword();
        var createUserResult = await _userManager.CreateAsync(employer, defaultPassword);
        if (!createUserResult.Succeeded)
        {
            string errorMsg = $"Failed to register employer. {createUserResult.Errors.FirstOrDefault()}";
            return StandardResponse<string>.Failed(null, errorMsg);
        }

        //Send Email to User with default password
        string mailSubject = "NLPC EPS Pensions: Welcome Message";
        string mailBody = CreateMailBody(employer.CompanyName, employer.Id);
        string senderName = "NLPC Enhanced Pension System";
        var emailParams = new EmailParams(senderEmail, mailSubject, mailBody, employer.Email, senderName, true);
        _emailService.SendEmail(emailParams);

        string successMsg = "Employer successfully registered. Kindly check mail for other details.";
        return StandardResponse<string>.Accepted(data: successMsg);
    }

    public async Task<StandardResponse<string>>
        SoftDeleteEmployerAsync(string employerId)
    {
        var employer = await _employerRepository.GetNonDeletedByCondition(e => e.Id == employerId)
            .FirstOrDefaultAsync();
        if (employer == null)
        {
            string? errorMsg = "Employer not found.";
            return StandardResponse<string>.Failed(errorMsg);
        }

        employer.IsDeleted = true;
        _employerRepository.Update(employer);
        await _employerRepository.SaveChangesAsync();

        return StandardResponse<string>.Accepted("Employer deleted successfully.");
    }

    public async Task<StandardResponse<string>>
        UpdateEmployerAsync(UpdateEmployerParams updateEmployerParams)
    {
        var employer = await _employerRepository.GetNonDeletedByCondition(e => e.Id == updateEmployerParams.employerId)
            .FirstOrDefaultAsync();
        if (employer == null)
            return StandardResponse<string>.Failed("Employer not found.");

        // Update employer details
        employer.CompanyName = updateEmployerParams.companyName ?? employer.CompanyName;
        employer.RegistrationNumber = updateEmployerParams.registrationNumber ?? employer.RegistrationNumber;
        employer.PhoneNumber = updateEmployerParams.phoneNumber ?? employer.PhoneNumber;
        employer.Email = updateEmployerParams.email ?? employer.Email;

        _employerRepository.Update(employer);
        await _employerRepository.SaveChangesAsync();

        return StandardResponse<string>.Success("Employer details updated successfully.");
    }

    public async Task<StandardResponse<string>>
        DeactivateEmployer(string employerId)
    {
        var employer = await _employerRepository.GetNonDeletedByCondition(e => e.Id == employerId)
            .FirstOrDefaultAsync();
        if (employer == null)
            return StandardResponse<string>.Failed("Employer not found.");

        employer.IsActive = false;

        _employerRepository.Update(employer);
        await _employerRepository.SaveChangesAsync();

        return StandardResponse<string>.Success("Employer deactivated successfully.");
    }

    public async Task<StandardResponse<string>>
        ActivateEmployer(string employerId)
    {
        var employer = await _employerRepository.GetNonDeletedByCondition(e => e.Id == employerId)
            .FirstOrDefaultAsync();
        if (employer == null)
            return StandardResponse<string>.Failed("Employer not found.");

        employer.IsActive = true;

        _employerRepository.Update(employer);
        await _employerRepository.SaveChangesAsync();

        return StandardResponse<string>.Success("Employer activated successfully.");
    }

    private string
        GenerateRandomPassword()
    {
        return Guid.NewGuid().ToString().Substring(0, 8);
    }

    private string CreateMailBody(string employerName, string employerId)
    {
        string mailBody = $@"
        <!DOCTYPE html>
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    color: #333;
                    padding: 20px;
                }}
                .container {{
                    max-width: 600px;
                    margin: 0 auto;
                    background: #ffffff;
                    padding: 20px;
                    border-radius: 10px;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                }}
                h2 {{
                    color: #2c3e50;
                }}
                .highlight {{
                    font-weight: bold;
                    color: #3498db;
                }}
                .footer {{
                    margin-top: 20px;
                    font-size: 12px;
                    color: #777;
                    text-align: center;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <h2>Welcome to Our Pension Management System, {employerName}!</h2>
                <p>We are thrilled to have you on board as a trusted employer managing your employees' pension plans with us.</p>
                <p><strong>Your Employer ID:</strong> <span class='highlight'>{employerId}</span></p>
                <p>With our platform, you will have seamless access to pension management tools, reports, and employee contribution tracking.</p>
                <p>If you have any questions, our support team is available to assist you.</p>
                <p>Thank you for choosing us!</p>
                <div class='footer'>
                    <p>&copy; {DateTime.Now.Year} NLPC EPS+. All Rights Reserved.</p>
                </div>
            </div>
        </body>
        </html>";
        return mailBody;
    }
}
