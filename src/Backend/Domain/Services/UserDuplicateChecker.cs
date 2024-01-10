using Domain.Models.Users;
using Domain.Repositories;

namespace Domain.Services;

public class UserDuplicateChecker : IUserDuplicateChecker
{
    private readonly IUserRepository userRepository;

    public UserDuplicateChecker(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task<bool> DuplicatedAsync(User user) =>
        await userRepository.GetAsync(user.Mail) is not null;
}