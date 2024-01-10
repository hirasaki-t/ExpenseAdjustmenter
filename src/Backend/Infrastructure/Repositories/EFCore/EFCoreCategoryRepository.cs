using Domain.Models.Categories;
using Domain.Models.ExpenseTypes;
using Domain.Repositories;
using Infrastructure.Repositories.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.EFCore;

public class EFCoreCategoryRepository : EFCoreRepositoryBase, ICategoryRepository
{
    public EFCoreCategoryRepository(ContextBase context) : base(context)
    {
    }

    public async Task<Category?> GetAsync(CategoryId id) =>
        await context.Categories.SingleOrDefaultAsync(x => x.Id == id);

    public async Task<Category?> GetAsync(CategoryName name) =>
        await context.Categories.SingleOrDefaultAsync(x => x.Name == name);

    public async Task AddAsync(Category category) => await context.Categories.AddAsync(category);

    public async Task UpdateAsync(Category category)
    {
        if (!await context.Categories.AnyAsync(x => x.Id == category.Id)) throw new ArgumentException("更新対象のオブジェクトが存在しません。");
        context.Categories.Update(category);
    }
}