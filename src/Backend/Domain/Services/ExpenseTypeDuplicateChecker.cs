using Domain.Models.ExpenseTypes;
using Domain.Repositories;

namespace Domain.Services;

public class ExpenseTypeDuplicateChecker : IExpenseTypeDuplicateChecker
{
    private readonly IExpenseTypeRepository expenseTypeRepository;

    public ExpenseTypeDuplicateChecker(IExpenseTypeRepository expenseTypeRepository)
    {
        this.expenseTypeRepository = expenseTypeRepository;
    }

    public async Task<bool> DuplicatedAsync(ExpenseType expenseType)
    {
        var result = await expenseTypeRepository.GetAsync(expenseType.Name);
        return result is not null && result.Id != expenseType.Id;
    }
}