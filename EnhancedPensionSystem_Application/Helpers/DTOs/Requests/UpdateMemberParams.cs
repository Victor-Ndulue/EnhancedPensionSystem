namespace EnhancedPensionSystem_Application.Helpers.DTOs.Requests;

public record UpdateMemberParams
(
    Guid MemberId,
    string? FirstName,
    string? LastName,
    string? userEmail,
    string? PhoneNumber,
    DateTime? DateOfBirth
);