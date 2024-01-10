using Domain;
using Domain.Models.Expenses;
using Domain.Repositories;
using Domain.Storages;

namespace Usecase.SundryExpenses;

public class SundryExpenseUpdater
{
    private readonly ISundryInformationRepository sundryInformationRepository;
    private readonly IExpenseRepository expenseRepository;
    private readonly IStorage storage;

    public SundryExpenseUpdater(ISundryInformationRepository sundryInformationRepository, IExpenseRepository expenseRepository, IStorage storage)
    {
        this.sundryInformationRepository = sundryInformationRepository;
        this.expenseRepository = expenseRepository;
        this.storage = storage;
    }

    public async Task UpdateAsync(string id, DateOnly date, string expenseTypeId, string? details, int participationNumber, int amount, string? submissionMethod, Stream? stream)
    {
        var sundryInformation = await sundryInformationRepository.GetAsync(new ExpenseId(id));
        DomainException.ThrowIfNotFound(sundryInformation, "諸経費情報");
        var expense = await expenseRepository.GetAsync(new ExpenseId(id));
        DomainException.ThrowIfNotFound(expense, "経費精算");

        var receiptId = (ReceiptId?)null;
        if (submissionMethod == SubmissionMethod.Electronics.Value && stream is not null)
        {
            if (expense.ReceiptId is not null) await storage.DeleteAsync("領収書", expense.ReceiptId.ToString());
            receiptId = new ReceiptId();
            await storage.SaveAsync("領収書", receiptId.ToString(), stream);
        }

        if (stream is null && expense.ReceiptId is not null)
            await storage.DeleteAsync("領収書", expense.ReceiptId.ToString());

        expense.Update(date, new(amount), submissionMethod is null ? null : new(submissionMethod), stream is null ? expense.ReceiptId : receiptId);
        sundryInformation.Update(new(expenseTypeId), details, new(participationNumber));

        await expenseRepository.UpdateAsync(expense);
        await sundryInformationRepository.UpdateAsync(sundryInformation);
        await expenseRepository.CommitAsync();
        await sundryInformationRepository.CommitAsync();
    }
}