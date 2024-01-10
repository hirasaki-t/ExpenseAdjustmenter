using Domain;
using Domain.Models.Users;
using Domain.Repositories;
using Domain.Services;
using Moq;
using Usecase.Users;

namespace UnitTest.Usecase.Users;

public class UserAdderTest
{
    [Test]
    public async Task ユーザーの追加に成功すること()
    {
        var userRepository = new Mock<IUserRepository>();
        var userNameSearcher = new Mock<IUserNameSearcher>();
        var userDuplicateChecker = new Mock<IUserDuplicateChecker>();

        userNameSearcher.Setup(x => x.SearchAsync(new Mail("yamada@google.com"))).ReturnsAsync(new UserName("山田 太郎"));
        userDuplicateChecker.Setup(x => x.DuplicatedAsync(It.IsAny<User>())).ReturnsAsync(false);

        await new UserAdder(userRepository.Object, userNameSearcher.Object, userDuplicateChecker.Object).AddAsync("yamada@google.com", true);

        userRepository.Verify(x => x.AddAsync(It.IsAny<User>()));
        userRepository.Verify(x => x.CommitAsync());
    }

    [Test]
    public async Task メールアドレスからユーザー名の取得に失敗した場合に例外になること()
    {
        var userNameSearcher = new Mock<IUserNameSearcher>();

        userNameSearcher.Setup(x => x.SearchAsync(new Mail("yamada@google.com"))).ReturnsAsync((UserName?)null);

        await FluentActions.Invoking(() => new UserAdder(new Mock<IUserRepository>().Object, userNameSearcher.Object, new Mock<IUserDuplicateChecker>().Object).AddAsync("yamada@google.com", false))
            .Should().ThrowAsync<DomainException>().WithMessage("対象のユーザー名が存在しません。");
    }

    [Test]
    public async Task 重複するアカウント名を追加しようとすると例外になること()
    {
        var userNameSearcher = new Mock<IUserNameSearcher>();
        var userDuplicateChecker = new Mock<IUserDuplicateChecker>();

        userNameSearcher.Setup(x => x.SearchAsync(new Mail("yamada@google.com"))).ReturnsAsync(new UserName("山田 太郎"));
        userDuplicateChecker.Setup(x => x.DuplicatedAsync(It.IsAny<User>())).ReturnsAsync(true);

        await FluentActions.Invoking(() => new UserAdder(new Mock<IUserRepository>().Object, userNameSearcher.Object, userDuplicateChecker.Object).AddAsync("yamada@google.com", false))
            .Should().ThrowAsync<DomainException>().WithMessage("メールアドレス[yamada@google.com]はすでに登録されています。");
    }
}