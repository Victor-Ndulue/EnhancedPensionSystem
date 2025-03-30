namespace EnhancedPensionSystem_Application.Helpers.DTOs.Responses;

public record EmployerResponse
(
    string id, string companyName, string regNumber,
    bool? isActive, string email, string phone
);