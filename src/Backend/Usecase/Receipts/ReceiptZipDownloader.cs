using System.IO.Compression;
using Domain;
using Domain.Models.Expenses;
using Domain.Repositories;
using Domain.Storages;

namespace Usecase.Receipts;

public record ReceiptZipDownloader
{
    private readonly IExpenseRepository expenseRepository;
    private readonly IUserRepository userRepository;
    private readonly IStorage storage;

    public ReceiptZipDownloader(IExpenseRepository expenseRepository, IUserRepository userRepository, IStorage storage)
    {
        this.expenseRepository = expenseRepository;
        this.userRepository = userRepository;
        this.storage = storage;
    }

    public async Task<byte[]> DownloadAsync(string[] expenseIds)
    {
        using var memoryStream = new MemoryStream();
        using var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true);
        var dictionary = await userRepository.GetDictionaryAsync();

        foreach(var expenseId in expenseIds)
        {
            var expense = await expenseRepository.GetAsync(new ExpenseId(expenseId));
            DomainException.ThrowIfNotFound(expense, "経費精算");
            if (expense.ReceiptId is null) continue;

            var zipArchiveEntry = zipArchive.CreateEntry($"./{dictionary[expense.UserId].Name}/ファイル.pdf");
            using var zipStream = zipArchiveEntry.Open();
            using var tempStream = new MemoryStream();
            (await storage.GetAsync("領収書", expense.ReceiptId.ToString())).CopyTo(tempStream);
            tempStream.Flush();
            tempStream.Seek(0, SeekOrigin.Begin);
            zipStream.Write(tempStream.ToArray(), 0, (int)tempStream.Length);
        }
        zipArchive.Dispose();
        await memoryStream.FlushAsync();
        return memoryStream.ToArray();
    }
}