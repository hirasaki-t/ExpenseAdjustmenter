using Domain;
using Domain.Models.Users;

namespace UnitTest.Domain.Models.Users;

public class UserNameTest
{
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("　")]
    public void ユーザー名を空にすることができない(string empty)
    {
        FluentActions.Invoking(() => new UserName(empty))
            .Should().Throw<DomainException>().WithMessage("ユーザー名を空にすることはできません。");
    }

    [Test]
    public void ユーザー名を生成できる()
    {
        var userName = new UserName("ユーザー名");
        userName.ToString().Should().Be("ユーザー名");
        userName.Should().Be(new UserName("ユーザー名"));
    }
}