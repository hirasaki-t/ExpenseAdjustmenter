using Domain;
using Domain.Models.Expenses;
using Domain.Models.TraveringInformations;
using Domain.Models.Users;
using Domain.Repositories;
using Domain.Services;
using Domain.Storages;

namespace Usecase.TraveringExpenses;

public class TraveringExpenseUpdater
{
    private readonly ITraveringInformationRepository traveringInformationRepository;
    private readonly IExpenseRepository expenseRepository;
    private readonly IStorage storage;

    public TraveringExpenseUpdater(
        ITraveringInformationRepository traveringInformationRepository, IExpenseRepository expenseRepository, IStorage storage)
    {
        this.traveringInformationRepository = traveringInformationRepository;
        this.expenseRepository = expenseRepository;
        this.storage = storage;
    }

    public async Task UpdateAsync(
        string id,
        DateOnly date,
        string workName,
        string? startSection,
        string? endSection,
        string categoryId,
        string? submissionMethod,
        Stream? stream,
        int amount,
        string? remarks)
    {
        var traveringInformation = await traveringInformationRepository.GetAsync(new ExpenseId(id));
        DomainException.ThrowIfNotFound(traveringInformation, "旅費・交通費情報");
        var expense = await expenseRepository.GetAsync(new ExpenseId(id));
        DomainException.ThrowIfNotFound(expense, "経費精算");

        var receiptId = (ReceiptId?)null;
        if (stream is not null)
        {
            if (expense.ReceiptId is not null) await storage.DeleteAsync("領収書", expense.ReceiptId.ToString());
            receiptId = new ReceiptId();
            await storage.SaveAsync("領収書", receiptId.ToString(), stream);
        }

        if (stream is null && expense.ReceiptId is not null)
            await storage.DeleteAsync("領収書", expense.ReceiptId.ToString());

        expense.Update(date, new(amount), submissionMethod is null ? null : new(submissionMethod), stream is null ? expense.ReceiptId : receiptId);
        traveringInformation.Update(new(categoryId), startSection is null ? null : new(startSection), endSection is null ? null : new(endSection), new(workName), remarks);

        await expenseRepository.UpdateAsync(expense);
        await traveringInformationRepository.UpdateAsync(traveringInformation);
        await expenseRepository.CommitAsync();
        await traveringInformationRepository.CommitAsync();
    }
}