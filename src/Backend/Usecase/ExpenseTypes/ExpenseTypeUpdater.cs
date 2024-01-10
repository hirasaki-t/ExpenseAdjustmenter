using Domain;
using Domain.Models.ExpenseTypes;
using Domain.Repositories;
using Domain.Services;

namespace Usecase.ExpenseTypes;

public class ExpenseTypeUpdater
{
    private readonly IExpenseTypeRepository expenseTypeRepository;
    private readonly IExpenseTypeDuplicateChecker expenseTypeDuplicateChecker;

    public ExpenseTypeUpdater(IExpenseTypeRepository expenseTypeRepository, IExpenseTypeDuplicateChecker expenseTypeDuplicateChecker)
    {
        this.expenseTypeRepository = expenseTypeRepository;
        this.expenseTypeDuplicateChecker = expenseTypeDuplicateChecker;
    }

    public async Task UpdateAsync(string id, string name, string? details, bool isReceipt, bool isActive)
    {
        var expenseType = await expenseTypeRepository.GetAsync(new ExpenseTypeId(id));
        DomainException.ThrowIfNotFound(expenseType, "経費種別");

        expenseType.UpdateName(new ExpenseTypeName(name));
        expenseType.UpdateDetals(details);
        expenseType.UpdateIsReceipt(isReceipt);
        expenseType.UpdateIsActive(isActive);

        if (await expenseTypeDuplicateChecker.DuplicatedAsync(expenseType))
            throw new DomainException($"経費種別名[{name}]はすでに登録されています。");

        await expenseTypeRepository.UpdateAsync(expenseType);
        await expenseTypeRepository.CommitAsync();
    }
}