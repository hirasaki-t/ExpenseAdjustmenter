using Domain.Models.ExpenseTypes;
using Domain.Repositories;
using Infrastructure.Repositories.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.EFCore;

public class EFCoreExpenseTypeRepository : EFCoreRepositoryBase, IExpenseTypeRepository
{
    public EFCoreExpenseTypeRepository(ContextBase context) : base(context)
    {
    }

    public async Task<ExpenseType?> GetAsync(ExpenseTypeId id) =>
        await context.ExpenseTypes.SingleOrDefaultAsync(x => x.Id == id);

    public async Task<ExpenseType?> GetAsync(ExpenseTypeName name) =>
        await context.ExpenseTypes.SingleOrDefaultAsync(x => x.Name == name);

    public async Task AddAsync(ExpenseType expenseType) => await context.ExpenseTypes.AddAsync(expenseType);

    public async Task UpdateAsync(ExpenseType expenseType)
    {
        if (!await context.ExpenseTypes.AnyAsync(x => x.Id == expenseType.Id)) throw new ArgumentException("更新対象のオブジェクトが存在しません。");
        context.ExpenseTypes.Update(expenseType);
    }
}