using Domain.Models.Expenses;

namespace UnitTest.Domain.Models.Expenses;

public class ExpenseIdTest
{
    [Test]
    public void IDの生成に成功する()
    {
        var id = new ExpenseId();
        var id2 = new ExpenseId(id.Value);
        id.Should().Be(id2);
    }

    [Test]
    public void String形式で出力される()
    {
        var id = new ExpenseId();
        id.ToString().Should().Be(id.Value.ToString());
    }
}