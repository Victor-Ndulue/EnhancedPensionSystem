using EnhancedPensionSystem_Application.Helpers.DTOs.Requests;
using EnhancedPensionSystem_Application.Services.Abstractions;
using EnhancedPensionSystem_Application.UnitOfWork.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace EnhancedPensionSystem_WebAPP.Controllers;

/// <summary>
/// Controller for managing employer-related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EmployersController : BaseController
{
    private readonly IEmployerService _employerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmployersController"/> class.
    /// </summary>
    /// <param name="serviceManager">The service manager that provides access to employer-related services.</param>
    public EmployersController(IServiceManager serviceManager)
    {
        _employerService = serviceManager.EmployerService;
    }

    /// <summary>
    /// Registers a new employer.
    /// </summary>
    /// <param name="employerParams">The employer registration details.</param>
    /// <returns>The result of the employer registration process.</returns>
    /// <response code="201">Employer registered successfully.</response>
    /// <response code="400">Invalid input data.</response>
    [HttpPost("register-employer")]
    public async Task<IActionResult> RegisterEmployerAsync([FromBody] CreateEmployerParams employerParams)
    {
        var result = await _employerService.RegisterEmployerAsync(employerParams);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Retrieves all registered employers.
    /// </summary>
    /// <returns>A list of employers.</returns>
    /// <response code="200">Successfully retrieved employers.</response>
    [HttpGet("all-employers")]
    public async Task<IActionResult> GetAllEmployersAsync()
    {
        var result = await _employerService.GetAllEmployersAsync();
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Retrieves an employer by their unique identifier.
    /// </summary>
    /// <param name="employerId">The employer's unique ID.</param>
    /// <returns>The employer details if found.</returns>
    /// <response code="200">Employer found and returned.</response>
    /// <response code="404">Employer not found.</response>
    [HttpGet("get-employer")]
    public async Task<IActionResult> GetEmployerByIdAsync([FromQuery] string employerId)
    {
        var result = await _employerService.GetEmployerByIdAsync(employerId);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Updates an existing employer's details.
    /// </summary>
    /// <param name="updateEmployerParams">The updated employer details.</param>
    /// <returns>The result of the update operation.</returns>
    /// <response code="200">Employer updated successfully.</response>
    /// <response code="400">Invalid input data.</response>
    /// <response code="404">Employer not found.</response>
    [HttpPut("update-employer")]
    public async Task<IActionResult> UpdateEmployerAsync([FromBody] UpdateEmployerParams updateEmployerParams)
    {
        var result = await _employerService.UpdateEmployerAsync(updateEmployerParams);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Deletes an employer (soft delete).
    /// </summary>
    /// <param name="employerId">The employer's unique ID.</param>
    /// <returns>The result of the delete operation.</returns>
    /// <response code="200">Employer deleted successfully.</response>
    /// <response code="404">Employer not found.</response>
    [HttpDelete("delete-employer")]
    public async Task<IActionResult> DeleteEmployerAsync([FromQuery] string employerId)
    {
        var result = await _employerService.SoftDeleteEmployerAsync(employerId);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Deactivates an employer.
    /// </summary>
    /// <param name="employerId">The employer's unique ID.</param>
    /// <returns>The result of the deactivation process.</returns>
    /// <response code="200">Employer deactivated successfully.</response>
    /// <response code="404">Employer not found.</response>
    [HttpPut("deactivate-employer")]
    public async Task<IActionResult> DeactivateEmployerAsync([FromQuery] string employerId)
    {
        var result = await _employerService.DeactivateEmployer(employerId);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Activates a previously deactivated employer.
    /// </summary>
    /// <param name="employerId">The employer's unique ID.</param>
    /// <returns>The result of the activation process.</returns>
    /// <response code="200">Employer activated successfully.</response>
    /// <response code="404">Employer not found.</response>
    [HttpPut("activate-employer")]
    public async Task<IActionResult> ActivateEmployerAsync([FromQuery] string employerId)
    {
        var result = await _employerService.ActivateEmployer(employerId);
        return StatusCode(result.StatusCode, result);
    }
}
