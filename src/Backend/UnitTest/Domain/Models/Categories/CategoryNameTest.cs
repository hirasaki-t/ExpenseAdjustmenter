using Domain;
using Domain.Models.Categories;

namespace UnitTest.Domain.Models.Categories;

public class CategoryNameTest
{
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("　")]
    public void 交通区分名を空にすることができない(string empty)
    {
        FluentActions.Invoking(() => new CategoryName(empty))
            .Should().Throw<DomainException>().WithMessage("交通区分名を空にすることはできません。");
    }

    [Test]
    public void 交通区分名を生成できる()
    {
        var categoryName = new CategoryName("交通区分名");
        categoryName.ToString().Should().Be("交通区分名");
        categoryName.Should().Be(new CategoryName("交通区分名"));
    }
}