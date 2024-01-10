using Domain;
using Domain.Models.ExpenseTypes;

namespace UnitTest.Domain.Models.ExpenseTypes;

public class ExpenseTypeNameTest
{
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("　")]
    public void 経費種別名を空にすることができない(string empty)
    {
        FluentActions.Invoking(() => new ExpenseTypeName(empty))
            .Should().Throw<DomainException>().WithMessage("経費種別名を空にすることはできません。");
    }

    [Test]
    public void 経費種別名を生成できる()
    {
        var userName = new ExpenseTypeName("経費種別名");
        userName.ToString().Should().Be("経費種別名");
        userName.Should().Be(new ExpenseTypeName("経費種別名"));
    }
}