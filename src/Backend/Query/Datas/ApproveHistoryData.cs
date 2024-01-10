namespace Query.Datas;

public record ApproveHistoryData(
    string Id,
    string expenseId,
    DateOnly Date,
    string UserId,
    string Type,
    int Amount,
    string? SubmissionMethod,
    string? ReceiptId,
    string? ReviewerId,
    string? Comment,
    string? Status,
    string? WorkName,
    string? StartSection,
    string? EndSection,
    string? CategoryId,
    string? Remarks,
    string? ExpenseTypeId,
    int? ParticipationNumber,
    string? Details);