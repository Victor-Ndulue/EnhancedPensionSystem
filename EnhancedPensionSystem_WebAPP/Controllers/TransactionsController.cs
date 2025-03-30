using EnhancedPensionSystem_Application.Services.Abstractions;
using EnhancedPensionSystem_Application.UnitOfWork.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace EnhancedPensionSystem_WebAPP.Controllers;

/// <summary>
/// Controller for handling financial transactions related to members.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TransactionsController : BaseController
{
    private readonly ITransactionService _transactionService;

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionsController"/> class.
    /// </summary>
    /// <param name="serviceManager">The service manager that provides access to transaction-related services.</param>
    public TransactionsController(IServiceManager serviceManager)
    {
        _transactionService = serviceManager.TransactionService;
    }

    /// <summary>
    /// Retrieves all transactions associated with a specific member.
    /// </summary>
    /// <param name="memberId">The unique identifier of the member.</param>
    /// <returns>A list of transactions related to the specified member.</returns>
    /// <response code="200">Transactions retrieved successfully.</response>
    /// <response code="404">No transactions found for the specified member.</response>
    [HttpGet("member-transactions")]
    public async Task<IActionResult> GetMemberTransactionsAsync([FromQuery] string memberId)
    {
        var result = await _transactionService.GetMemberTransactionsAsync(memberId);
        return StatusCode(result.StatusCode, result);
    }
}
