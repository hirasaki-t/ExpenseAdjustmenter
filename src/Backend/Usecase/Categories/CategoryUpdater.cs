using Domain;
using Domain.Models.Categories;
using Domain.Models.ExpenseTypes;
using Domain.Repositories;
using Domain.Services;

namespace Usecase.Categories;

public class CategoryUpdater
{
    private readonly ICategoryRepository categoryRepository;
    private readonly ICategoryDuplicateChecker categoryDuplicateChecker;

    public CategoryUpdater(ICategoryRepository categoryRepository, ICategoryDuplicateChecker categoryDuplicateChecker)
    {
        this.categoryRepository = categoryRepository;
        this.categoryDuplicateChecker = categoryDuplicateChecker;
    }

    public async Task UpdateAsync(string id, string name, string? details, bool isReceipt, bool isActive)
    {
        var category = await categoryRepository.GetAsync(new CategoryId(id));
        DomainException.ThrowIfNotFound(category, "交通区分");

        category.UpdateName(new CategoryName(name));
        category.UpdateDetals(details);
        category.UpdateIsReceipt(isReceipt);
        category.UpdateIsActive(isActive);

        if (await categoryDuplicateChecker.DuplicatedAsync(category))
            throw new DomainException($"交通区分名[{name}]はすでに登録されています。");

        await categoryRepository.UpdateAsync(category);
        await categoryRepository.CommitAsync();
    }
}