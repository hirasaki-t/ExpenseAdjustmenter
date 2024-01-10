using Domain.Models.Expenses;
using Domain.Models.TraveringInformations;

namespace Domain.Repositories;

public interface ITraveringInformationRepository : IWritableRepository
{
    Task<TraveringInformation?> GetAsync(ExpenseId id);

    Task AddAsync(TraveringInformation traveringInformation);

    Task UpdateAsync(TraveringInformation traveringInformation);

    Task DeleteAsync(ExpenseId id);
}