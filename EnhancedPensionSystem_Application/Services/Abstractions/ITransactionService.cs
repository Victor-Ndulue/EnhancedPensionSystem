using EnhancedPensionSystem_Application.Helpers.DTOs.Requests;
using EnhancedPensionSystem_Application.Helpers.DTOs.Responses;
using EnhancedPensionSystem_Application.Helpers.ObjectFormatter;
using EnhancedPensionSystem_Domain.Enums;

namespace EnhancedPensionSystem_Application.Services.Abstractions;

public interface ITransactionService
{
    Task<StandardResponse<string>> AddTransactionAsync(AddTransactionParams transactionParams);
    Task<StandardResponse<IEnumerable<TransactionResponse>>> GetMemberTransactionsAsync(string memberId);
    Task<StandardResponse<TransactionResponse>> GetTransactionByIdAsync(string transactionId);
    Task<StandardResponse<string>> UpdateTransactionStatusAsync(UpdateTransactionStatusParams transactionStatusParams);
}
