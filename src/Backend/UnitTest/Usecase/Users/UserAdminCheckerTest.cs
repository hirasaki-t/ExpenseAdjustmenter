using Domain;
using Domain.Models.Users;
using Domain.Repositories;
using Domain.Services;
using Moq;
using Usecase.Users;

namespace UnitTest.Usecase.Users;

public class UserAdminCheckerTest
{
    [Test]
    public async Task 管理者の場合はTrueが返ってくること()
    {
        var userRepository = new Mock<IUserRepository>();
        var loginUserGetter = new Mock<ILoginUserGetter>();
        var user = new User(new("鈴木 一郎"), new("suzuki@google.com"), true, true);

        loginUserGetter.Setup(x => x.GetMailAsync()).ReturnsAsync("suzuki@google.com");
        userRepository.Setup(x => x.GetAsync(new Mail("suzuki@google.com"))).ReturnsAsync(user);

        (await new UserAdminChecker(userRepository.Object, loginUserGetter.Object).CheckAsync()).Should().BeTrue();
    }

    [Test]
    public async Task 管理者でない場合はFalseが返ってくること()
    {
        var userRepository = new Mock<IUserRepository>();
        var loginUserGetter = new Mock<ILoginUserGetter>();
        var user = new User(new("鈴木 一郎"), new("suzuki@google.com"), false, true);

        loginUserGetter.Setup(x => x.GetMailAsync()).ReturnsAsync("suzuki@google.com");
        userRepository.Setup(x => x.GetAsync(new Mail("suzuki@google.com"))).ReturnsAsync(user);

        (await new UserAdminChecker(userRepository.Object, loginUserGetter.Object).CheckAsync()).Should().BeFalse();
    }

    [Test]
    public async Task ユーザー情報の取得に失敗した場合に例外になること()
    {
        var userRepository = new Mock<IUserRepository>();
        var loginUserGetter = new Mock<ILoginUserGetter>();

        loginUserGetter.Setup(x => x.GetMailAsync()).ReturnsAsync("suzuki@google.com");
        userRepository.Setup(x => x.GetAsync(new Mail("suzuki@google.com"))).ReturnsAsync((User?)null);

        await FluentActions.Invoking(() => new UserAdminChecker(userRepository.Object, loginUserGetter.Object).CheckAsync())
            .Should().ThrowAsync<DomainException>().WithMessage("対象のユーザーが存在しません。");
    }
}