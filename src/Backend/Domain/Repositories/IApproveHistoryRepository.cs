using Domain.Models.ApproveHistories;
using Domain.Models.Expenses;

namespace Domain.Repositories;

public interface IApproveHistoryRepository : IWritableRepository
{
    Task<IDictionary<ExpenseId, ApproveStatus>> GetLatestStatusDictionaryAsync();

    Task AddRangeAsync(IEnumerable<ApproveHistory> approveHistories);

    Task AddAsync(ApproveHistory approveHistory);

    Task DeleteRangeAsync(ExpenseId expenseId);
}