using Domain.Models.Expenses;
using Domain.Models.TraveringInformations;
using Domain.Repositories;
using Infrastructure.Repositories.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.EFCore;

public class EFCoreTraveringInformationRepository : EFCoreRepositoryBase, ITraveringInformationRepository
{
    public EFCoreTraveringInformationRepository(ContextBase context) : base(context)
    {
    }

    public async Task<TraveringInformation?> GetAsync(ExpenseId id) =>
        await context.TraveringInformations.SingleOrDefaultAsync(x => x.ExpenseId == id);

    public async Task AddAsync(TraveringInformation traveringInformation) =>
        await context.TraveringInformations.AddAsync(traveringInformation);

    public async Task UpdateAsync(TraveringInformation traveringInformation)
    {
        if (!await context.TraveringInformations.AnyAsync(x => x.ExpenseId == traveringInformation.ExpenseId)) throw new ArgumentException("更新対象のオブジェクトが存在しません。");
        context.TraveringInformations.Update(traveringInformation);
    }

    public async Task DeleteAsync(ExpenseId id)
    {
        var traveringInformation = await GetAsync(id);
        if (traveringInformation is null) throw new ArgumentException("削除対象のオブジェクトが存在しません。");
        context.TraveringInformations.Remove(traveringInformation);
    }
}