using Domain.Models.Expenses;
using Domain.Models.Users;
using Domain.Repositories;
using Infrastructure.Repositories.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.EFCore;

public class EFCoreExpenseRepository : EFCoreRepositoryBase, IExpenseRepository
{
    public EFCoreExpenseRepository(ContextBase context) : base(context)
    {
    }

    public async Task<IEnumerable<Expense>> GetsAsync(UserId userId, DateOnly date) =>
        (await context.Expenses.ToArrayAsync()).Where(x => x.UserId == userId && x.Date >= date && x.Date < date.AddMonths(1));

    public async Task<Expense?> GetAsync(ExpenseId id) =>
        await context.Expenses.SingleOrDefaultAsync(x => x.Id == id);

    public async Task<Expense?> GetAsync(ReceiptId receiptId) =>
        await context.Expenses.SingleOrDefaultAsync(x => x.ReceiptId == receiptId);

    public async Task AddAsync(Expense expense) => await context.Expenses.AddAsync(expense);

    public async Task UpdateAsync(Expense expense)
    {
        if (!await context.Expenses.AnyAsync(x => x.Id == expense.Id)) throw new ArgumentException("更新対象のオブジェクトが存在しません。");
        context.Expenses.Update(expense);
    }

    public async Task DeleteAsync(ExpenseId id)
    {
        var expense = await GetAsync(id);
        if (expense is null) throw new ArgumentException("削除対象のオブジェクトが存在しません。");
        context.Expenses.Remove(expense);
    }
}