using Domain.Models.Categories;
using Domain.Models.ExpenseTypes;

namespace UnitTest.Domain.Models.Categories;

public class CategoryTest
{
    [Test]
    public void 正しい情報で生成される()
    {
        var name = new CategoryName("交通区分名");
        var category = new Category(name, "備考", true);

        category.Should().BeEquivalentTo(new
        {
            Name = name,
            Details = "備考",
            IsReceipt = true
        });
    }

    [Test]
    public void 名前が更新されること()
    {
        var category = new Category(new CategoryName("交通区分名"), "説明", true);
        category.UpdateName(new CategoryName("新しい交通区分名"));
        category.Name.Should().Be(new CategoryName("新しい交通区分名"));
    }

    [Test]
    public void 説明が更新されること()
    {
        var category = new Category(new CategoryName("交通区分名"), "説明", true);
        category.UpdateDetals(null);
        category.Details.Should().BeNull();
    }

    [Test]
    public void 領収書フラグが更新されること()
    {
        var category = new Category(new CategoryName("交通区分名"), "説明", false);
        category.IsReceipt.Should().BeFalse();
        category.UpdateIsReceipt(true);
        category.IsReceipt.Should().BeTrue();
        category.UpdateIsReceipt(false);
        category.IsReceipt.Should().BeFalse();
    }

    [Test]
    public void 有効フラグが更新されること()
    {
        var category = new Category(new CategoryId(), new CategoryName("交通区分名"), "説明", false, false);
        category.IsActive.Should().BeFalse();
        category.UpdateIsActive(true);
        category.IsActive.Should().BeTrue();
        category.UpdateIsActive(false);
        category.IsActive.Should().BeFalse();
    }
}