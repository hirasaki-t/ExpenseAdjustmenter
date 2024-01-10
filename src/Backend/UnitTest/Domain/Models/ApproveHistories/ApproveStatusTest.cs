using Domain;
using Domain.Models.ApproveHistories;
using Domain.Models.Expenses;

namespace UnitTest.Domain.Models.ApproveHistories;

public class ApproveStatusTest
{
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("　")]
    public void ステータスを空にすることができない(string empty)
    {
        FluentActions.Invoking(() => new ApproveStatus(empty))
            .Should().Throw<DomainException>().WithMessage("ステータスを空にすることはできません。");
    }

    [Test]
    public void 未登録のステータスは生成できない()
    {
        FluentActions.Invoking(() => new ApproveStatus("誤り"))
            .Should().Throw<DomainException>().WithMessage("有効なステータスを指定してください。");
    }

    [TestCase("申請中")]
    [TestCase("承認")]
    [TestCase("否認")]
    [TestCase("保留")]
    public void ステータスを生成できる(string value)
    {
        var approveStatus = new ApproveStatus(value);
        approveStatus.ToString().Should().Be(value);
        approveStatus.Should().Be(new ApproveStatus(value));
    }
}