using Domain.Models.Users;

namespace UnitTest.Domain.Models.Users;

public class UserIdTest
{
    [Test]
    public void IDの生成に成功する()
    {
        var id = new UserId();
        var id2 = new UserId(id.Value);
        id.Should().Be(id2);
    }

    [Test]
    public void String形式で出力される()
    {
        var id = new UserId();
        id.ToString().Should().Be(id.Value.ToString());
    }
}