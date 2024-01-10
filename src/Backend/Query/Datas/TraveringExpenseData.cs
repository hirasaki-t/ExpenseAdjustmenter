namespace Query.Datas;

public record TraveringExpenseData(string Id, DateOnly Date, string? StartSection, string? EndSection, string CategoryId, string WorkName, string? Remarks, int Amount, string? ReceiptId, string? SubmissionMethod, string? ReviewerId, string? Comment, string? Status);