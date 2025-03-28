namespace EnhancedPensionSystem_Application.Helpers.DTOs.Requests;

public record UpdateMemberParams
(
    string? MemberId,
    string? FirstName,
    string? LastName,
    string? userEmail,
    string? phoneNumber,
    DateTime? dateOfBirth
);