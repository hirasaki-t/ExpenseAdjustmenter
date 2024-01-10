using Domain;
using Domain.Models.Users;
using Domain.Repositories;
using Domain.Services;

namespace Usecase.Users;

public class UserAdder
{
    private readonly IUserRepository userRepository;
    private readonly IUserNameSearcher userNameSearcher;
    private readonly IUserDuplicateChecker userDuplicateChecker;

    public UserAdder(IUserRepository userRepository, IUserNameSearcher userNameSearcher, IUserDuplicateChecker userDuplicateChecker)
    {
        this.userRepository = userRepository;
        this.userNameSearcher = userNameSearcher;
        this.userDuplicateChecker = userDuplicateChecker;
    }

    public async Task<string> AddAsync(string mail, bool isAdmin)
    {
        var userName = await userNameSearcher.SearchAsync(new(mail));
        DomainException.ThrowIfNotFound(userName, "ユーザー名");

        var user = new User(userName, new(mail), isAdmin, true);
        if (await userDuplicateChecker.DuplicatedAsync(user))
            throw new DomainException($"メールアドレス[{mail}]はすでに登録されています。");

        await userRepository.AddAsync(user);
        await userRepository.CommitAsync();
        return user.Id.ToString();
    }
}