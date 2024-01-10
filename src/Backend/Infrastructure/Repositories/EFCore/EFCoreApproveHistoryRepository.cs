using Domain.Models.ApproveHistories;
using Domain.Models.Expenses;
using Domain.Repositories;
using Infrastructure.Repositories.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.EFCore;

public class EFCoreApproveHistoryRepository : EFCoreRepositoryBase, IApproveHistoryRepository
{
    public EFCoreApproveHistoryRepository(ContextBase context) : base(context)
    {
    }

    public async Task<IDictionary<ExpenseId, ApproveStatus>> GetLatestStatusDictionaryAsync()
    {
        var groups = await context.ApproveHistories.ToArrayAsync();
        return groups.GroupBy(x => x.ExpenseId).ToDictionary(x => x.Key, x => x.MaxBy(x => x.Date)!.Status);
    }

    public async Task AddRangeAsync(IEnumerable<ApproveHistory> approveHistories) =>
        await context.ApproveHistories.AddRangeAsync(approveHistories);

    public async Task AddAsync(ApproveHistory approveHistory) =>
        await context.ApproveHistories.AddAsync(approveHistory);

    public async Task DeleteRangeAsync(ExpenseId expenseId)
    {
        var approveHistories = await context.ApproveHistories.Where(x => x.ExpenseId == expenseId).ToArrayAsync();
        context.RemoveRange(approveHistories);
    }
}