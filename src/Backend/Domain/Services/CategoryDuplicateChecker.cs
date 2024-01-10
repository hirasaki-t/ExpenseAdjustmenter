using Domain.Models.Categories;
using Domain.Models.ExpenseTypes;
using Domain.Repositories;

namespace Domain.Services;

public class CategoryDuplicateChecker : ICategoryDuplicateChecker
{
    private readonly ICategoryRepository categoryRepository;

    public CategoryDuplicateChecker(ICategoryRepository categoryRepository)
    {
        this.categoryRepository = categoryRepository;
    }

    public async Task<bool> DuplicatedAsync(Category category)
    {
        var result = await categoryRepository.GetAsync(category.Name);
        return result is not null && result.Id != category.Id;
    }
}