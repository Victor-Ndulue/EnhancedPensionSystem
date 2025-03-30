using EnhancedPensionSystem_Application.Helpers.DTOs.Requests;
using EnhancedPensionSystem_Application.Helpers.DTOs.Responses;
using EnhancedPensionSystem_Application.Helpers.ObjectFormatter;

namespace EnhancedPensionSystem_Application.Services.Abstractions;

public interface IEmployerService
{
    Task<bool> ConfirmEmployerExistsAsync(string? employerId);
    Task<string?> GetEmployerNameByIdAsync(string? employerId);
    Task<Dictionary<string, string>> GetEmployersNameDictionaryAsync();
    Task<StandardResponse<string>> RegisterEmployerAsync(CreateEmployerParams createEmployerParams);
    Task<StandardResponse<IEnumerable<EmployerResponse>>> GetAllEmployersAsync();
    Task<StandardResponse<EmployerResponse>> GetEmployerByIdAsync(string employerId);
    Task<StandardResponse<string>> UpdateEmployerAsync(UpdateEmployerParams updateEmployerParams);
    Task<StandardResponse<string>> SoftDeleteEmployerAsync(string employerId);
    Task<StandardResponse<string>> DeactivateEmployer(string employerId);
    Task<StandardResponse<string>> ActivateEmployer(string employerId);
}
