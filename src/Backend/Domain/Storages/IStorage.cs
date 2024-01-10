namespace Domain.Storages;

public interface IStorage
{
    Task CreateFolderAsync(string folderPath);
    
    Task SaveAsync(string folderPath, string fileName, Stream stream);

    Task<Stream> GetAsync(string folderPath, string fileName);

    Task DeleteAsync(string folderPath, string fileName);
}