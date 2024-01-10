using Domain.Models.ExpenseTypes;

namespace Domain.Repositories;

public interface IExpenseTypeRepository : IWritableRepository
{
    Task<ExpenseType?> GetAsync(ExpenseTypeId id);

    Task<ExpenseType?> GetAsync(ExpenseTypeName name);

    Task AddAsync(ExpenseType expenseType);

    Task UpdateAsync(ExpenseType expenseType);
}