using Domain;
using Domain.Models.Expenses;
using Domain.Models.TraveringInformations;
using Domain.Models.Users;
using Domain.Repositories;
using Domain.Services;
using Domain.Storages;

namespace Usecase.TraveringExpenses;

public class TraveringExpenseAdder
{
    private readonly ITraveringInformationRepository traveringInformationRepository;
    private readonly IExpenseRepository expenseRepository;
    private readonly IUserRepository userRepository;
    private readonly ILoginUserGetter loginUserGetter;
    private readonly IStorage storage;

    public TraveringExpenseAdder(
        ITraveringInformationRepository traveringInformationRepository,
        IExpenseRepository expenseRepository,
        IUserRepository userRepository,
        ILoginUserGetter loginUserGetter,
        IStorage storage)
    {
        this.traveringInformationRepository = traveringInformationRepository;
        this.expenseRepository = expenseRepository;
        this.userRepository = userRepository;
        this.loginUserGetter = loginUserGetter;
        this.storage = storage;
    }

    public async Task<string> AddAsync(
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
        var receiptId = (ReceiptId?)null;
        if (submissionMethod == SubmissionMethod.Electronics.Value && stream is not null)
        {
            receiptId = new ReceiptId();
            await storage.CreateFolderAsync("領収書");
            await storage.SaveAsync("領収書", receiptId.ToString(), stream);
        }

        var mail = await loginUserGetter.GetMailAsync();
        var user = await userRepository.GetAsync(new Mail(mail));
        DomainException.ThrowIfNotFound(user, "ユーザー");

        var traveringInformation = new TraveringInformation(new(categoryId), startSection is null ? null : new(startSection), endSection is null ? null : new(endSection), new(workName), remarks);
        var expense = new Expense(user.Id, date, new(amount), submissionMethod is null ? null : new(submissionMethod), receiptId, traveringInformation);

        await expenseRepository.AddAsync(expense);
        await traveringInformationRepository.AddAsync(traveringInformation);
        await expenseRepository.CommitAsync();
        await traveringInformationRepository.CommitAsync();

        return expense.Id.ToString();
    }
}