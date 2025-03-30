namespace EnhancedPensionSystem_Application.Helpers.DTOs.Requests;

/// <summary>
/// Parameters for updating an employer's details.
/// </summary>
/// <param name="employerId">The unique identifier of the employer.</param>
/// <param name="companyName">The updated company name.</param>
/// <param name="registrationNumber">The updated registration number.</param>
/// <param name="email">The updated email address.</param>
/// <param name="phoneNumber">The updated phone number.</param>
public record UpdateEmployerParams
(
    string? employerId,
    string? companyName,
    string? registrationNumber,
    string? email,
    string? phoneNumber
);