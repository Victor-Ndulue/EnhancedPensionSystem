namespace EnhancedPensionSystem_Application.Helpers.DTOs.Requests;

/// <summary>
/// Parameters for creating a new employer.
/// </summary>
/// <param name="companyName">The name of the company.</param>
/// <param name="registrationNumber">The registration number of the company.</param>
/// <param name="email">The email address of the employer.</param>
/// <param name="phoneNumber">The phone number of the employer.</param>
public record CreateEmployerParams
(
    string companyName,
    string registrationNumber,
    string email,
    string phoneNumber
);