namespace Query.Datas;

public record SundryExpenseData(string Id, DateOnly Date, string ExpenseTypeId, string? Details, int ParticipationNumber, int Amount, string? ReceiptId, string? SubmissionMethod, string? ReviewerId, string? Comment, string? Status);