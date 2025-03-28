namespace EnhancedPensionSystem_Application.Helpers.DTOs.Requests;

public record CreateMemberParams
(
    string? FirstName,
    string? LastName,
    string? userEmail,
    string? phoneNumber,
    string? employerId,
    DateTime? dateOfBirth
);
