namespace Domain.Repositories;

public interface IWritableRepository
{
    Task CommitAsync();
}