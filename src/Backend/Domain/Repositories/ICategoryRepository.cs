using Domain.Models.Categories;

namespace Domain.Repositories;

public interface ICategoryRepository : IWritableRepository
{
    Task<Category?> GetAsync(CategoryId id);

    Task<Category?> GetAsync(CategoryName name);

    Task AddAsync(Category category);

    Task UpdateAsync(Category category);
}