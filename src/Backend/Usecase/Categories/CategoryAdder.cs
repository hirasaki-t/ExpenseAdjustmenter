using Domain;
using Domain.Models.Categories;
using Domain.Models.ExpenseTypes;
using Domain.Repositories;
using Domain.Services;

namespace Usecase.Categories;

public class CategoryAdder
{
    private readonly ICategoryRepository categoryRepository;
    private readonly ICategoryDuplicateChecker categoryDuplicateChecker;

    public CategoryAdder(ICategoryRepository categoryRepository, ICategoryDuplicateChecker categoryDuplicateChecker)
    {
        this.categoryRepository = categoryRepository;
        this.categoryDuplicateChecker = categoryDuplicateChecker;
    }

    public async Task<string> AddAsync(string name, string? details, bool isReceipt)
    {
        var category = new Category(new CategoryName(name), details, isReceipt);
        if (await categoryDuplicateChecker.DuplicatedAsync(category))
            throw new DomainException($"交通区分名[{name}]はすでに登録されています。");

        await categoryRepository.AddAsync(category);
        await categoryRepository.CommitAsync();
        return category.Id.ToString();
    }
}