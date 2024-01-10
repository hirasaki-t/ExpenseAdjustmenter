using Domain;
using Domain.Models.Expenses;

namespace UnitTest.Domain.Models.Expenses;

public class AmountTest
{
    [TestCase(-1)]
    public void 範囲外の数値では生成できない(int value)
    {
        FluentActions.Invoking(() => new Amount(value))
            .Should().Throw<DomainException>().WithMessage("金額は0円以上を入力してください。");
    }

    [TestCase(0)]
    [TestCase(99)]
    public void 金額を生成できる(int value)
    {
        var amount = new Amount(value);
        amount.Value.Should().Be(value);
        amount.Should().Be(new Amount(value));
    }
}