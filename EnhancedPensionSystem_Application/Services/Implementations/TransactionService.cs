using EnhancedPensionSystem_Application.Helpers.DTOs.Requests;
using EnhancedPensionSystem_Application.Helpers.DTOs.Responses;
using EnhancedPensionSystem_Application.Helpers.ObjectFormatter;
using EnhancedPensionSystem_Application.Services.Abstractions;
using EnhancedPensionSystem_Domain.Enums;
using EnhancedPensionSystem_Domain.Models;
using EnhancedPensionSystem_Infrastructure.Repository.Implementations;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EnhancedPensionSystem_Application.Services.Implementations;

public sealed class TransactionService : ITransactionService
{
    private readonly IGenericRepository<Transaction> _transactionRepository;

    public TransactionService(IGenericRepository<Transaction> transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<StandardResponse<string>> 
        AddTransactionAsync(AddTransactionParams transactionParams)
    {
        var transaction = new Transaction
        {
            MemberId = transactionParams.MemberId,
            ContributionId = transactionParams.ContributionId,
            Amount = transactionParams.Amount,
            ContributionType = transactionParams.ContributionType,
            TransactionStatus = transactionParams.Status
        };

        await _transactionRepository.AddAsync(transaction);
        await _transactionRepository.SaveChangesAsync();

        string successMsg = "Transaction recorded successfully.";
        return StandardResponse<string>.Success(successMsg);
    }

    public async Task<StandardResponse<IEnumerable<TransactionResponse>>> 
        GetMemberTransactionsAsync(string memberId)
    {
        var transactions = await _transactionRepository.GetNonDeletedByCondition(t => t.MemberId == memberId)
            .Select(t => new TransactionResponse
            (
                t.Id, t.MemberId!, t.ContributionId!, t.Amount,
                t.ContributionType, t.TransactionStatus, t.CreatedAt
            )).AsNoTracking()
            .ToListAsync();

        return StandardResponse<IEnumerable<TransactionResponse>>.Success(transactions);
    }

    public async Task<StandardResponse<TransactionResponse>> 
        GetTransactionByIdAsync(string transactionId)
    {
        var transaction = await _transactionRepository.GetNonDeletedByCondition(t => t.Id == transactionId)
             .Select(t => new TransactionResponse
            (
                t.Id, t.MemberId!, t.ContributionId!, t.Amount,
                t.ContributionType, t.TransactionStatus, t.CreatedAt
            )).AsNoTracking()
            .FirstOrDefaultAsync();

        return StandardResponse<TransactionResponse>.Success(transaction);
    }

    public async Task<StandardResponse<string>> 
        UpdateTransactionStatusAsync(UpdateTransactionStatusParams transactionStatusParams)
    {
        var transaction = await _transactionRepository.GetNonDeletedByCondition(t => t.Id == transactionStatusParams.transactionId)
            .FirstOrDefaultAsync();

        if (transaction is null)
        {
            string errorMsg = "Transaction not found.";
            return StandardResponse<string>.Failed(errorMsg);
        }

        transaction.TransactionStatus = transactionStatusParams.status;
        await _transactionRepository.SaveChangesAsync();

        return StandardResponse<string>.Success("Transaction status updated successfully.");
    }
}
