namespace EnhancedPensionSystem_Application.Services.Abstractions;

public interface IEmployerService
{
    Task<string?> GetEmployerNameByIdAsync(string employerId);
    Task<Dictionary<string, string>> GetEmployersNameDictionaryAsync();
}
