using Domain;
using Domain.Models.Expenses;
using Domain.Repositories;
using Domain.Storages;

namespace Usecase.SundryExpenses;

public class SundryExpenseDeleter
{
    private readonly ISundryInformationRepository sundryInformationRepository;
    private readonly IExpenseRepository expenseRepository;
    private readonly IApproveHistoryRepository approveHistoryRepository;
    private readonly IStorage storage;

    public SundryExpenseDeleter(
        ISundryInformationRepository sundryInformationRepository,
        IExpenseRepository expenseRepository,
        IApproveHistoryRepository approveHistoryRepository,
        IStorage storage)
    {
        this.sundryInformationRepository = sundryInformationRepository;
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
        await sundryInformationRepository.DeleteAsync(new(id));
        await expenseRepository.DeleteAsync(new(id));
        await sundryInformationRepository.CommitAsync();
        await expenseRepository.CommitAsync();
    }
}