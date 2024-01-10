using Domain.Models.Expenses;
using Domain.Models.SundryInformations;

namespace Domain.Repositories;

public interface ISundryInformationRepository : IWritableRepository
{
    Task<SundryInformation?> GetAsync(ExpenseId id);

    Task AddAsync(SundryInformation sundryInformation);

    Task UpdateAsync(SundryInformation sundryInformation);

    Task DeleteAsync(ExpenseId id);
}