namespace EnhancedPensionSystem_Application.Helpers.DTOs.Requests;

public record CreateEmployerParams
(
    string companyName,
    string registrationNumber,
    string email, string phoneNumber
);