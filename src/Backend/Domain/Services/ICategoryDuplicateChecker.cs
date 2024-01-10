using Domain.Models.Categories;

namespace Domain.Services;

public interface ICategoryDuplicateChecker
{
    Task<bool> DuplicatedAsync(Category category);
}