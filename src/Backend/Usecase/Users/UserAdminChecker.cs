using Domain;
using Domain.Models.Users;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.MicrosoftGraphs;

namespace Usecase.Users;

public class UserAdminChecker
{
    private readonly IUserRepository userRepository;
    private readonly ILoginUserGetter loginUserGetter;

    public UserAdminChecker(IUserRepository userRepository, ILoginUserGetter loginUserGetter)
    {
        this.userRepository = userRepository;
        this.loginUserGetter = loginUserGetter;
    }

    public async Task<bool> CheckAsync()
    {
        var mail = await loginUserGetter.GetMailAsync();
        var user = await userRepository.GetAsync(new Mail(mail));
        DomainException.ThrowIfNotFound(user, "ユーザー");
        return user.IsAdmin;
    }
}