using Domain;
using Domain.Models.Users;
using Domain.Repositories;

namespace Usecase.Users;

public class UserUpdater
{
    private readonly IUserRepository userRepository;

    public UserUpdater(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task UpdateAsync(string id, bool isAdmin, bool isActive)
    {
        var user = await userRepository.GetAsync(new UserId(id));
        DomainException.ThrowIfNotFound(user, "ユーザー");

        user.UpdateIsAdmin(isAdmin);
        user.UpdateIsActive(isActive);

        await userRepository.UpdateAsync(user);
        await userRepository.CommitAsync();
    }
}