using EnhancedPensionSystem_Application.Helpers.DTOs.Requests;
using EnhancedPensionSystem_Application.Services.Abstractions;
using EnhancedPensionSystem_Application.UnitOfWork.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace EnhancedPensionSystem_WebAPP.Controllers;

/// <summary>
/// Controller for managing member-related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MembersController : BaseController
{
    private readonly IMemberService _memberService;

    /// <summary>
    /// Initializes a new instance of the <see cref="MembersController"/> class.
    /// </summary>
    /// <param name="serviceManager">The service manager that provides access to member-related services.</param>
    public MembersController(IServiceManager serviceManager)
    {
        _memberService = serviceManager.MemberService;
    }

    /// <summary>
    /// Registers a new member.
    /// </summary>
    /// <param name="createMemberParams">The member registration details.</param>
    /// <returns>The result of the member registration process.</returns>
    /// <response code="201">Member registered successfully.</response>
    /// <response code="400">Invalid input data.</response>
    [HttpPost("register-member")]
    public async Task<IActionResult> RegisterMemberAsync([FromBody] CreateMemberParams createMemberParams)
    {
        var result = await _memberService.RegisterMemberAsync(createMemberParams);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Retrieves a member by their unique identifier.
    /// </summary>
    /// <param name="memberId">The member's unique ID.</param>
    /// <returns>The member details if found.</returns>
    /// <response code="200">Member found and returned.</response>
    /// <response code="404">Member not found.</response>
    [HttpGet("get-member")]
    public async Task<IActionResult> GetMemberByIdAsync([FromQuery] string memberId)
    {
        var result = await _memberService.GetMemberByIdAsync(memberId);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Retrieves all members belonging to a specific employer.
    /// </summary>
    /// <param name="employerId">The employer's unique ID.</param>
    /// <returns>A list of employer members.</returns>
    /// <response code="200">Successfully retrieved employer members.</response>
    [HttpGet("get-employer-members")]
    public async Task<IActionResult> GetEmployerMembersAsync([FromQuery] string employerId)
    {
        var result = await _memberService.GetEmployerMembersAsync(employerId);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Retrieves all registered members.
    /// </summary>
    /// <returns>A list of all members.</returns>
    /// <response code="200">Successfully retrieved all members.</response>
    [HttpGet("get-all-employers")]
    public async Task<IActionResult> GetAllEmployersAsync()
    {
        var result = await _memberService.GetAllMembersAsync();
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Updates an existing member's details.
    /// </summary>
    /// <param name="updateMemberParams">The updated member details.</param>
    /// <returns>The result of the update operation.</returns>
    /// <response code="200">Member updated successfully.</response>
    /// <response code="400">Invalid input data.</response>
    /// <response code="404">Member not found.</response>
    [HttpPut("update-member")]
    public async Task<IActionResult> UpdateMemberAsync([FromBody] UpdateMemberParams updateMemberParams)
    {
        var result = await _memberService.UpdateMemberAsync(updateMemberParams);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Deletes a member (soft delete).
    /// </summary>
    /// <param name="memberId">The member's unique ID.</param>
    /// <returns>The result of the delete operation.</returns>
    /// <response code="200">Member deleted successfully.</response>
    /// <response code="404">Member not found.</response>
    [HttpDelete("delete-member")]
    public async Task<IActionResult> DeleteMemberAsync([FromQuery] string memberId)
    {
        var result = await _memberService.SoftDeleteMemberAsync(memberId);
        return StatusCode(result.StatusCode, result);
    }
}
