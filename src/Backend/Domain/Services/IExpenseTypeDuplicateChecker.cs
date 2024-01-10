using Domain.Models.ExpenseTypes;

namespace Domain.Services;

public interface IExpenseTypeDuplicateChecker
{
    Task<bool> DuplicatedAsync(ExpenseType expenseType);
}