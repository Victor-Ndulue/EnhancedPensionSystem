using EnhancedPensionSystem_Application.Services.Abstractions;
using EnhancedPensionSystem_Application.UnitOfWork.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace EnhancedPensionSystem_WebAPP.Controllers;

/// <summary>
/// Controller for managing benefit eligibility operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BenefitEligibilitiesController : BaseController
{
    private readonly IBenefitEligibilityService _benefitEligibilityService;

    /// <summary>
    /// Initializes a new instance of the <see cref="BenefitEligibilitiesController"/> class.
    /// </summary>
    /// <param name="serviceManager">The service manager that provides access to the benefit eligibility service.</param>
    public BenefitEligibilitiesController(ServiceManager serviceManager)
    {
        _benefitEligibilityService = serviceManager.BenefitEligibilityService;
    }

    /// <summary>
    /// Checks if a member is eligible for benefits.
    /// </summary>
    /// <param name="memberId">The unique identifier of the member.</param>
    /// <returns>
    /// Returns a response indicating whether the member is eligible for benefits.
    /// </returns>
    /// <response code="200">Returns the eligibility status of the member.</response>
    /// <response code="400">If the request is invalid or the member ID is missing.</response>
    /// <response code="404">If the member is not found.</response>
    [HttpGet("check-member-eligibility")]
    public async Task<IActionResult> CheckMemberBenefitEligibilityAsync([FromQuery] string memberId)
    {
        var result = await _benefitEligibilityService.CheckMemberEligibilityAsync(memberId);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Retrieves a list of all eligible members.
    /// </summary>
    /// <returns>
    /// A list of members who qualify for benefits.
    /// </returns>
    /// <response code="200">Returns the list of eligible members.</response>
    /// <response code="404">If no eligible members are found.</response>
    [HttpGet("all-eligible-members")]
    public async Task<IActionResult> GetAllEligibleMembersAsync()
    {
        var result = await _benefitEligibilityService.GetAllEligibleMembersAsync();
        return StatusCode(result.StatusCode, result);
    }
}
