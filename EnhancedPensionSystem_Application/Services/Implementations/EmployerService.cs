using EnhancedPensionSystem_Application.Services.Abstractions;
using EnhancedPensionSystem_Domain.Models;
using EnhancedPensionSystem_Infrastructure.Repository.Implementations;
using Microsoft.EntityFrameworkCore;

namespace EnhancedPensionSystem_Application.Services.Implementations;

public sealed class EmployerService : IEmployerService
{
    private readonly IGenericRepository<Employer> _employerRepository;

    public EmployerService(IGenericRepository<Employer> employerRepository)
    {
        _employerRepository = employerRepository;
    }

    public async Task<string?> GetEmployerNameByIdAsync(string employerId)
    {
        string? employerName = await _employerRepository.GetNonDeletedByCondition(employer => employer.Id == employerId)
            .Select(employer=>employer.CompanyName).FirstOrDefaultAsync();
        return employerName;
    }

    public async Task<Dictionary<string, string>>
        GetEmployersNameDictionaryAsync()
    {
        var employersNameDictionary = await _employerRepository.GetAllNonDeleted()
            .ToDictionaryAsync(employer=>employer.Id, employer=>employer.CompanyName!);
        return employersNameDictionary;
    }
}
