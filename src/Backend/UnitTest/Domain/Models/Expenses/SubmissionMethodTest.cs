using Domain;
using Domain.Models.Expenses;

namespace UnitTest.Domain.Models.Expenses;

public class SubmissionMethodTest
{
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("　")]
    public void 提出方法を空にすることができない(string empty)
    {
        FluentActions.Invoking(() => new SubmissionMethod(empty))
            .Should().Throw<DomainException>().WithMessage("提出方法を空にすることはできません。");
    }

    [Test]
    public void 未登録の提出方法は生成できない()
    {
        FluentActions.Invoking(() => new SubmissionMethod("神/郵送"))
            .Should().Throw<DomainException>().WithMessage("有効な提出方法を指定してください。");
    }

    [TestCase("紙/持参")]
    [TestCase("紙/郵送")]
    [TestCase("電子")]
    public void 提出方法を生成できる(string value)
    {
        var submissionMethod = new SubmissionMethod(value);
        submissionMethod.ToString().Should().Be(value);
        submissionMethod.Should().Be(new SubmissionMethod(value));
    }
}