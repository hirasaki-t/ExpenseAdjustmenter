using Domain;
using Domain.Models.Users;
using Domain.Repositories;
using Moq;
using Usecase.Users;

namespace UnitTest.Usecase.Users;

public class UserUpdaterTest
{
    [Test]
    public async Task ユーザーの更新に成功すること()
    {
        var userRepository = new Mock<IUserRepository>();
        var user = new User(new("山田 太郎"), new("yamada@google.com"), true, true);

        userRepository.Setup(x => x.GetAsync(user.Id)).ReturnsAsync(user);

        await new UserUpdater(userRepository.Object).UpdateAsync(user.Id.ToString(), false, false);

        userRepository.Verify(x => x.UpdateAsync(user));
        userRepository.Verify(x => x.CommitAsync());
    }

    [Test]
    public async Task ユーザーIDからユーザーが取得できない場合は例外になること()
    {
        var userRepository = new Mock<IUserRepository>();

        userRepository.Setup(x => x.GetAsync(It.IsAny<UserId>())).ReturnsAsync((User?)null);

        await FluentActions.Invoking(() => new UserUpdater(userRepository.Object).UpdateAsync(Ulid.NewUlid().ToString(), true, true))
            .Should().ThrowAsync<DomainException>().WithMessage("対象のユーザーが存在しません。");
    }
}