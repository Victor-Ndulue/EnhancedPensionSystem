namespace EnhancedPensionSystem_Application.Helpers.DTOs.Responses;

public record MemberResponse
(
    string? id,
    string? firstName,
    string? lastName,
    string? email,
    string? phoneNumber,
    string? employerName,
    DateTime? dateOfBirth
);