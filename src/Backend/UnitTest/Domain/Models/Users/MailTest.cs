using Domain;
using Domain.Models.Users;

namespace UnitTest.Domain.Models.Users;

public class MailTest
{
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("　")]
    public void メールアドレスを空にすることができない(string empty)
    {
        FluentActions.Invoking(() => new Mail(empty))
            .Should().Throw<DomainException>().WithMessage("メールアドレスを空にすることはできません。");
    }

    [Test]
    public void メールアドレスを生成できる()
    {
        var mail = new Mail("メールアドレス");
        mail.ToString().Should().Be("メールアドレス");
        mail.Should().Be(new Mail("メールアドレス"));
    }
}