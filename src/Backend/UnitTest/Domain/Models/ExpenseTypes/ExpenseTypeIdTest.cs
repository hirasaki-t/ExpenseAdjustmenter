using Domain.Models.ExpenseTypes;

namespace UnitTest.Domain.Models.ExpenseTypes;

public class ExpenseTypeIdTest
{
    [Test]
    public void IDの生成に成功する()
    {
        var id = new ExpenseTypeId();
        var id2 = new ExpenseTypeId(id.Value);
        id.Should().Be(id2);
    }

    [Test]
    public void String形式で出力される()
    {
        var id = new ExpenseTypeId();
        id.ToString().Should().Be(id.Value.ToString());
    }
}