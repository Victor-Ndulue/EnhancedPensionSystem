namespace EnhancedPensionSystem_Application.Helpers.DTOs.Requests;

/// <summary>
/// Parameters for updating an existing member's details.
/// </summary>
/// <param name="MemberId">The unique identifier of the member.</param>
/// <param name="FirstName">The updated first name of the member.</param>
/// <param name="LastName">The updated last name of the member.</param>
/// <param name="userEmail">The updated email address of the member.</param>
/// <param name="phoneNumber">The updated phone number of the member.</param>
/// <param name="dateOfBirth">The updated date of birth of the member.</param>
public record UpdateMemberParams
(
    string? MemberId,
    string? FirstName,
    string? LastName,
    string? userEmail,
    string? phoneNumber,
    DateTime? dateOfBirth
);