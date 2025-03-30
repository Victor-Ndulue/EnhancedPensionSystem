using EnhancedPensionSystem_Application.Helpers.DTOs.Requests;
using EnhancedPensionSystem_Application.Services.Abstractions;
using EnhancedPensionSystem_Application.UnitOfWork.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace EnhancedPensionSystem_WebAPP.Controllers;

/// <summary>
/// Controller for managing member contributions.
/// </summary>
[ApiController]
[Route("api/contributions")]
public class ContributionsController : BaseController
{
    private readonly IContributionService _contributionService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContributionsController"/> class.
    /// </summary>
    /// <param name="serviceManager">The service manager that provides access to contribution services.</param>
    public ContributionsController(IServiceManager serviceManager)
    {
        _contributionService = serviceManager.ContributionService;
    }

    /// <summary>
    /// Adds a new contribution for a member.
    /// </summary>
    /// <param name="addContributionParams">The contribution details including member ID, amount, date, and type.</param>
    /// <returns>Returns the status of the contribution addition.</returns>
    /// <response code="201">Contribution successfully added.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost("add-contribution")]
    public async Task<IActionResult> AddContributionAsync([FromForm] AddContributionParams addContributionParams)
    {
        var result = await _contributionService.AddContributionAsync(addContributionParams);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Retrieves a breakdown of a member's contributions.
    /// </summary>
    /// <param name="memberId">The unique identifier of the member.</param>
    /// <returns>Returns a detailed breakdown of the member's contributions.</returns>
    /// <response code="200">Successfully retrieved the contribution breakdown.</response>
    /// <response code="400">Invalid member ID provided.</response>
    /// <response code="404">Member contributions not found.</response>
    [HttpPost("member-contribution-breakdown")]
    public async Task<IActionResult> GetMemberContributionsAsync([FromQuery] string memberId)
    {
        var result = await _contributionService.GetMemberContributionsAsync(memberId);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Gets the total contributions made by a specific member.
    /// </summary>
    /// <param name="memberId">The unique identifier of the member.</param>
    /// <returns>Returns the total contribution amount for the member.</returns>
    /// <response code="200">Successfully retrieved the total contributions.</response>
    /// <response code="400">Invalid member ID provided.</response>
    /// <response code="404">No contributions found for the member.</response>
    [HttpGet("member-total-contribution")]
    public async Task<IActionResult> GetMemberTotalConributionAsync([FromQuery] string memberId)
    {
        var result = await _contributionService.GetTotalContributionsAsync(memberId);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Retrieves a list of all contributions.
    /// </summary>
    /// <returns>Returns all contributions available in the system.</returns>
    /// <response code="200">Successfully retrieved all contributions.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet("all-contributions")]
    public async Task<IActionResult> GetAllContributionsAsync()
    {
        var result = await _contributionService.GetAllContributionsAsync();
        return StatusCode(result.StatusCode, result);
    }
}