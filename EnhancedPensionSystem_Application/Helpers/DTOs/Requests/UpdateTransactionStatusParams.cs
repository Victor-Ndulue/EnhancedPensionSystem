using EnhancedPensionSystem_Domain.Enums;

namespace EnhancedPensionSystem_Application.Helpers.DTOs.Requests;

public record UpdateTransactionStatusParams
(
    string transactionId, TransactionStatus status
);

