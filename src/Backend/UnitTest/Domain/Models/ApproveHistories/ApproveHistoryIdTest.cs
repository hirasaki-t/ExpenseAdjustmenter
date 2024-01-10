using Domain.Models.ApproveHistories;

namespace UnitTest.Domain.Models.ApproveHistories;

public class ApproveHistoryIdTest
{
    [Test]
    public void IDの生成に成功する()
    {
        var id = new ApproveHistoryId();
        var id2 = new ApproveHistoryId(id.Value);
        id.Should().Be(id2);
    }

    [Test]
    public void String形式で出力される()
    {
        var id = new ApproveHistoryId();
        id.ToString().Should().Be(id.Value.ToString());
    }
}