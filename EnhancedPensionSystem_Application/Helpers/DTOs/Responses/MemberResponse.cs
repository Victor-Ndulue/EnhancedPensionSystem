namespace EnhancedPensionSystem_Application.Helpers.DTOs.Responses;

public record MemberResponse
(
    Guid id,
    string firstName,
    string lastName,
    string email,
    string phoneNumber,
    string employerName,
    DateTime dateOfBirth
);