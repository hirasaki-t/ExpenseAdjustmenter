using Azure.Storage.Files.Shares;
using Domain;
using Domain.Storages;

namespace Infrastructure.Storages;

public class AzureStorage : IStorage
{
    private readonly string connectionString;
    private readonly string storageName;

    public AzureStorage(string connectionString, string storageName)
    {
        this.connectionString = connectionString;
        this.storageName = storageName;
    }

    public async Task CreateFolderAsync(string folderPath)
    {
        var shareClient = new ShareClient(connectionString, storageName);
        if (!await shareClient.ExistsAsync())
            throw new DomainException("存在しないAzureStorageが指定されています。");

        var directory = shareClient.GetDirectoryClient($"/{folderPath}");
        if (!await directory.ExistsAsync()) await directory.CreateAsync();
    }

    public async Task SaveAsync(string folderPath, string fileName, Stream stream)
    {
        var shareClient = new ShareClient(connectionString, storageName);
        if (!await shareClient.ExistsAsync())
            throw new DomainException("存在しないAzureStorageが指定されています。");

        var directory = shareClient.GetDirectoryClient($"/{folderPath}");
        var fileInfo = await directory.CreateFileAsync(fileName, stream.Length);
        stream.Seek(0, SeekOrigin.Begin);
        await fileInfo.Value.UploadAsync(stream);
    }

    public async Task<Stream> GetAsync(string folderPath, string fileName)
    {
        var shareClient = new ShareClient(connectionString, storageName);
        if (!await shareClient.ExistsAsync())
            throw new DomainException("存在しないAzureStorageが指定されています。");

        var directory = shareClient.GetDirectoryClient($"/{folderPath}");
        var file = directory.GetFileClient(fileName);
        if (file is null) throw new DomainException($"ファイル名[{fileName}]がAzureStorageに存在していません。");

        var result = await file.DownloadAsync();
        return result.Value.Content;
    }

    public async Task DeleteAsync(string folderPath, string fileName)
    {
        var shareClient = new ShareClient(connectionString, storageName);
        if (!await shareClient.ExistsAsync())
            throw new DomainException("存在しないAzureStorageが指定されています。");

        var directory = shareClient.GetDirectoryClient($"/{folderPath}");
        var file = directory.GetFileClient(fileName);
        if (file is null) throw new DomainException($"ファイル名[{fileName}]がAzureStorageに存在していません。");

        await file.DeleteAsync();
    }
}