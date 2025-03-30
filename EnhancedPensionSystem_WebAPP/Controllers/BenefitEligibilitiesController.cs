using EnhancedPensionSystem_Application.Services.Abstractions;
using EnhancedPensionSystem_Application.UnitOfWork.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace EnhancedPensionSystem_WebAPP.Controllers;

public class BenefitEligibilitiesController : BaseController
{
    private readonly IBenefitEligibilityService _benefitEligibilityService;

    public BenefitEligibilitiesController(ServiceManager serviceManager)
    {
        _benefitEligibilityService = serviceManager.BenefitEligibilityService;
    }

    [HttpGet("check-member-eligibility")]
    public async Task<IActionResult> CheckMemberBenefitEligibilityAsync([FromQuery] string memberId)
    {
        var result = await _benefitEligibilityService.CheckMemberEligibilityAsync(memberId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("all-eligible-members")]
    public async Task<IActionResult> GetAllEligibleMembersAsync()
    {
        var result = await _benefitEligibilityService.GetAllEligibleMembersAsync();
        return StatusCode(result.StatusCode, result);
    }
}
