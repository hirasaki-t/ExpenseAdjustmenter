using Domain;
using Domain.Models.Expenses;
using Domain.Repositories;
using Domain.Storages;

namespace Usecase.Receipts;

public class ReceiptDownloader
{
    private readonly IExpenseRepository expenseRepository;
    private readonly IUserRepository userRepository;
    private readonly IStorage storage;

    public ReceiptDownloader(IExpenseRepository expenseRepository, IUserRepository userRepository, IStorage storage)
    {
        this.expenseRepository = expenseRepository;
        this.userRepository = userRepository;
        this.storage = storage;
    }

    public async Task<(Stream, string)> DownloadAsync(string receiptId)
    {
        var expense = await expenseRepository.GetAsync(new ReceiptId(receiptId));
        DomainException.ThrowIfNotFound(expense, "経費精算");

        var user = await userRepository.GetAsync(expense.UserId);
        DomainException.ThrowIfNotFound(user, "ユーザー");

        var stream = await storage.GetAsync("領収書", receiptId);
        return (stream, $"領収書_{user.Name.ToString().Replace(" ", "")}_{expense.Date.ToString("yyyyMMdd")}.pdf");
    }
}