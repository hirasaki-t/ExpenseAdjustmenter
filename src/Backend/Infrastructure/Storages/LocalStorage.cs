using Domain.Storages;

namespace Infrastructure.Storages;

public class LocalStorage : IStorage
{
    private readonly string directoryPath;

    public LocalStorage(string directoryPath)
    {
        this.directoryPath = directoryPath;
    }

    public Task CreateFolderAsync(string folderPath)
    {
        var folderDirectory = Path.Combine(directoryPath, folderPath);
        if (!Directory.Exists(folderDirectory)) Directory.CreateDirectory(folderDirectory);
        return Task.CompletedTask;
    }

    public async Task SaveAsync(string folderPath, string fileName, Stream stream)
    {
        var folderDirectory = Path.Combine(directoryPath, folderPath);
        var fileStream = new FileStream($"{folderDirectory}/{fileName}", FileMode.Create);
        await stream.CopyToAsync(fileStream);
        fileStream.Close();
    }

    public Task DeleteAsync(string folderPath, string fileName)
    {
        throw new NotImplementedException();
    }

    public Task<Stream> GetAsync(string folderPath, string fileName)
    {
        throw new NotImplementedException();
    }
}