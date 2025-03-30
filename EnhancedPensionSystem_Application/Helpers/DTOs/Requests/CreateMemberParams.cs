namespace EnhancedPensionSystem_Application.Helpers.DTOs.Requests;

/// <summary>
/// Parameters for creating a new member.
/// </summary>
/// <param name="FirstName">The first name of the member.</param>
/// <param name="LastName">The last name of the member.</param>
/// <param name="userEmail">The email address of the member.</param>
/// <param name="phoneNumber">The phone number of the member.</param>
/// <param name="employerId">The unique identifier of the employer the member belongs to.</param>
/// <param name="dateOfBirth">The date of birth of the member.</param>
public record CreateMemberParams
(
    string? FirstName,
    string? LastName,
    string? userEmail,
    string? phoneNumber,
    string? employerId,
    DateTime? dateOfBirth
);
