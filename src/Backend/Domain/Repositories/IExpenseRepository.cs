
using Domain.Models.Expenses;
using Domain.Models.Users;

namespace Domain.Repositories;

public interface IExpenseRepository : IWritableRepository
{
    Task<IEnumerable<Expense>> GetsAsync(UserId userId, DateOnly date);

    Task<Expense?> GetAsync(ExpenseId id);

    Task<Expense?> GetAsync(ReceiptId receiptId);

    Task AddAsync(Expense expense);

    Task UpdateAsync(Expense expense);

    Task DeleteAsync(ExpenseId id);
}