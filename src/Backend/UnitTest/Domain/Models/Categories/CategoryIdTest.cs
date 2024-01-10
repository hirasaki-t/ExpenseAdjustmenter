using Domain.Models.Categories;

namespace UnitTest.Domain.Models.Categories;

public class CategoryIdTest
{
    [Test]
    public void IDの生成に成功する()
    {
        var id = new CategoryId();
        var id2 = new CategoryId(id.Value);
        id.Should().Be(id2);
    }

    [Test]
    public void String形式で出力される()
    {
        var id = new CategoryId();
        id.ToString().Should().Be(id.Value.ToString());
    }
}