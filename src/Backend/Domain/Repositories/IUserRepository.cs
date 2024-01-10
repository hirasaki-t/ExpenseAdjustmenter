using Domain.Models.Users;

namespace Domain.Repositories;

public interface IUserRepository : IWritableRepository
{
    Task<User?> GetAsync(UserId id);

    Task<User?> GetAsync(Mail mail);

    Task<IDictionary<UserId, User>> GetDictionaryAsync();

    Task AddAsync(User user);

    Task UpdateAsync(User user);
}