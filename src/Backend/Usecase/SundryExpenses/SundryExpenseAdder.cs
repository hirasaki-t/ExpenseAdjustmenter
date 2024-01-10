using Domain;
using Domain.Models.Expenses;
using Domain.Models.ExpenseTypes;
using Domain.Models.SundryInformations;
using Domain.Models.Users;
using Domain.Repositories;
using Domain.Services;
using Domain.Storages;

namespace Usecase.SundryExpenses;

public class SundryExpenseAdder
{
    private readonly ISundryInformationRepository sundryInformationRepository;
    private readonly IExpenseRepository expenseRepository;
    private readonly IUserRepository userRepository;
    private readonly ILoginUserGetter loginUserGetter;
    private readonly IStorage storage;

    public SundryExpenseAdder(
        ISundryInformationRepository sundryInformationRepository,
        IExpenseRepository expenseRepository,
        IUserRepository userRepository,
        ILoginUserGetter loginUserGetter,
        IStorage storage)
    {
        this.sundryInformationRepository = sundryInformationRepository;
        this.expenseRepository = expenseRepository;
        this.userRepository = userRepository;
        this.loginUserGetter = loginUserGetter;
        this.storage = storage;
    }

    public async Task<string> AddAsync(DateOnly date, string expenseTypeId, string? details, int participationNumber, int amount, string? submissionMethod, Stream? stream)
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

        var sundryInformation = new SundryInformation(new(expenseTypeId), details, new(participationNumber));
        var expense = new Expense(user.Id, date, new(amount), submissionMethod is null ? null : new(submissionMethod), receiptId is null ? null : receiptId, sundryInformation);

        await sundryInformationRepository.AddAsync(sundryInformation);
        await expenseRepository.AddAsync(expense);
        await sundryInformationRepository.CommitAsync();
        await expenseRepository.CommitAsync();

        return expense.Id.ToString();
    }
}