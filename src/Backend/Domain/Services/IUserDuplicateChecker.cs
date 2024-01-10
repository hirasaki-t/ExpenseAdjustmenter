using Domain.Models.Users;

namespace Domain.Services;

public interface IUserDuplicateChecker
{
    Task<bool> DuplicatedAsync(User user);
}