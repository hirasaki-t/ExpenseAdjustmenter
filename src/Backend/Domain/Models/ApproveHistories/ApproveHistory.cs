using Domain.Models.Expenses;
using Domain.Models.Users;

namespace Domain.Models.ApproveHistories;

public class ApproveHistory
{
    public ApproveHistory(ExpenseId expenseId, UserId userId, string? comment, ApproveStatus status) :
        this(new ApproveHistoryId(), expenseId, userId, DateTime.Now, comment, status)
    { }

    public ApproveHistory(ApproveHistoryId id, ExpenseId expenseId, UserId userId, DateTime date, string? comment, ApproveStatus status)
    {
        Id = id;
        ExpenseId = expenseId;
        UserId = userId;
        Date = date;
        Comment = comment;
        Status = status;
    }

    public ApproveHistoryId Id { get; }

    public ExpenseId ExpenseId { get; }

    public UserId UserId { get; }

    public DateTime Date { get; }

    public string? Comment { get; }

    public ApproveStatus Status { get; }
}