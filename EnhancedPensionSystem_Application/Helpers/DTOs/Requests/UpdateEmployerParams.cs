namespace EnhancedPensionSystem_Application.Helpers.DTOs.Requests;

public record UpdateEmployerParams
(
    string? employerId,
    string? companyName,
    string? registrationNumber,
    string? email, string? phoneNumber
);
