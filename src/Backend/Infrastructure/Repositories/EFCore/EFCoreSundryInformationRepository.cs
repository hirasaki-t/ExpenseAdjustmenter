using Domain.Models.Expenses;
using Domain.Models.SundryInformations;
using Domain.Models.TraveringInformations;
using Domain.Repositories;
using Infrastructure.Repositories.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.EFCore;

public class EFCoreSundryInformationRepository : EFCoreRepositoryBase, ISundryInformationRepository
{
    public EFCoreSundryInformationRepository(ContextBase context) : base(context)
    {
    }

    public async Task<SundryInformation?> GetAsync(ExpenseId id) =>
        await context.SundryInformations.SingleOrDefaultAsync(x => x.ExpenseId == id);

    public async Task AddAsync(SundryInformation sundryInformation) =>
        await context.SundryInformations.AddAsync(sundryInformation);

    public async Task UpdateAsync(SundryInformation sundryInformation)
    {
        if (!await context.SundryInformations.AnyAsync(x => x.ExpenseId == sundryInformation.ExpenseId)) throw new ArgumentException("更新対象のオブジェクトが存在しません。");
        context.SundryInformations.Update(sundryInformation);
    }

    public async Task DeleteAsync(ExpenseId id)
    {
        var sundryInformation = await GetAsync(id);
        if (sundryInformation is null) throw new ArgumentException("削除対象のオブジェクトが存在しません。");
        context.SundryInformations.Remove(sundryInformation);
    }
}