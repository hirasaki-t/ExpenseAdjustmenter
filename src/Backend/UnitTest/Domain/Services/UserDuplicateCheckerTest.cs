using Domain.Models.Users;
using Domain.Repositories;
using Domain.Services;
using Moq;

namespace UnitTest.Domain.Services;

public class UserDuplicateCheckerTest
{
    [Test]
    public async Task アカウント名がすでに存在した場合はTRUEが返却されること()
    {
        var userRepository = new Mock<IUserRepository>();
        var user = new User(new UserName("ユーザー名"), new Mail("メールアドレス"), true, true);

        userRepository.Setup(x => x.GetAsync(user.Mail)).ReturnsAsync(user);

        (await new UserDuplicateChecker(userRepository.Object).DuplicatedAsync(user)).Should().BeTrue();
    }

    [Test]
    public async Task アカウント名がまだ存在していない場合はFALSEが返却されること()
    {
        var userRepository = new Mock<IUserRepository>();
        var user = new User(new UserName("ユーザー名"), new Mail("メールアドレス"), true, true);

        userRepository.Setup(x => x.GetAsync(It.IsAny<Mail>())).ReturnsAsync((User?)null);

        (await new UserDuplicateChecker(userRepository.Object).DuplicatedAsync(user)).Should().BeFalse();
    }
}