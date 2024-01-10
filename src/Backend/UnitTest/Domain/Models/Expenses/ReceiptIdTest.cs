using Domain.Models.Expenses;

namespace UnitTest.Domain.Models.Expenses;

public record ReceiptIdTest
{
    [Test]
    public void IDの生成に成功する()
    {
        var id = new ReceiptId();
        var id2 = new ReceiptId(id.Value);
        id.Should().Be(id2);
    }

    [Test]
    public void String形式で出力される()
    {
        var id = new ReceiptId();
        id.ToString().Should().Be(id.Value.ToString());
    }
}