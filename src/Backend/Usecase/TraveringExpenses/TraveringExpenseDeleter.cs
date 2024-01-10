using Domain;
using Domain.Models.Expenses;
using Domain.Repositories;
using Domain.Services;
using Domain.Storages;

namespace Usecase.TraveringExpenses;

public class TraveringExpenseDeleter
{
    private readonly ITraveringInformationRepository traveringInformationRepository;
    private readonly IExpenseRepository expenseRepository;
    private readonly IApproveHistoryRepository approveHistoryRepository;
    private readonly IStorage storage;

    public TraveringExpenseDeleter(
        ITraveringInformationRepository traveringInformationRepository,
        IExpenseRepository expenseRepository,
        IApproveHistoryRepository approveHistoryRepository,
        IStorage storage)
    {
        this.traveringInformationRepository = traveringInformationRepository;
        this.expenseRepository = expenseRepository;
        this.approveHistoryRepository = approveHistoryRepository;
        this.storage = storage;
    }

    public async Task DeleteAsync(string id)
    {
        var expense = await expenseRepository.GetAsync(new ExpenseId(id));
        DomainException.ThrowIfNotFound(expense, "経費精算");

        if (expense.ReceiptId is not null)
            await storage.DeleteAsync("領収書", expense.ReceiptId.ToString());

        await approveHistoryRepository.DeleteRangeAsync(new(id));
        await approveHistoryRepository.CommitAsync();
        await traveringInformationRepository.DeleteAsync(new(id));
        await expenseRepository.DeleteAsync(new(id));
        await traveringInformationRepository.CommitAsync();
        await expenseRepository.CommitAsync();
    }
}