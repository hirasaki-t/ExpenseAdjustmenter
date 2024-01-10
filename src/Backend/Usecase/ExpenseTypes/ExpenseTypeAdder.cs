using Domain;
using Domain.Models.ExpenseTypes;
using Domain.Repositories;
using Domain.Services;

namespace Usecase.ExpenseTypes;

public class ExpenseTypeAdder
{
    private readonly IExpenseTypeRepository expenseTypeRepository;
    private readonly IExpenseTypeDuplicateChecker expenseTypeDuplicateChecker;

    public ExpenseTypeAdder(IExpenseTypeRepository expenseTypeRepository, IExpenseTypeDuplicateChecker expenseTypeDuplicateChecker)
    {
        this.expenseTypeRepository = expenseTypeRepository;
        this.expenseTypeDuplicateChecker = expenseTypeDuplicateChecker;
    }

    public async Task<string> AddAsync(string name, string? details, bool isReceipt)
    {
        var expenseType = new ExpenseType(new ExpenseTypeName(name), details, isReceipt);
        if (await expenseTypeDuplicateChecker.DuplicatedAsync(expenseType))
            throw new DomainException($"経費種別名[{name}]はすでに登録されています。");

        await expenseTypeRepository.AddAsync(expenseType);
        await expenseTypeRepository.CommitAsync();
        return expenseType.Id.ToString();
    }
}