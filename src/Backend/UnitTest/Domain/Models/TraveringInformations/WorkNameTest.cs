using Domain;
using Domain.Models.Expenses;
using Domain.Models.TraveringInformations;

namespace UnitTest.Domain.Models.TraveringInformations;

public class WorkNameTest
{
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("　")]
    public void 業務名を空にすることができない(string empty)
    {
        FluentActions.Invoking(() => new WorkName(empty))
            .Should().Throw<DomainException>().WithMessage("業務名を空にすることはできません。");
    }

    [Test]
    public void 未登録の業務名は生成できない()
    {
        FluentActions.Invoking(() => new WorkName("自己都合"))
            .Should().Throw<DomainException>().WithMessage("有効な業務名を指定してください。");
    }

    [TestCase("社内業務")]
    [TestCase("営業")]
    [TestCase("現場業務")]
    public void 業務名を生成できる(string value)
    {
        var workName = new WorkName(value);
        workName.ToString().Should().Be(value);
        workName.Should().Be(new WorkName(value));
    }
}