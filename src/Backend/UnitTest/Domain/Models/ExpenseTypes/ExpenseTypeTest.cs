using Domain.Models.ExpenseTypes;

namespace UnitTest.Domain.Models.ExpenseTypes;

public class ExpenseTypeTest
{
    [Test]
    public void 正しい情報で生成される()
    {
        var name = new ExpenseTypeName("経費種別名");
        var expenseType = new ExpenseType(name, "備考", true);

        expenseType.Should().BeEquivalentTo(new
        {
            Name = name,
            Details = "備考",
            IsReceipt = true
        });
    }

    [Test]
    public void 名前が更新されること()
    {
        var expenseType = new ExpenseType(new ExpenseTypeName("経費種別名"), "説明", true);
        expenseType.UpdateName(new ExpenseTypeName("新しい経費種別名"));
        expenseType.Name.Should().Be(new ExpenseTypeName("新しい経費種別名"));
    }

    [Test]
    public void 説明が更新されること()
    {
        var expenseType = new ExpenseType(new ExpenseTypeName("経費種別名"), "説明", true);
        expenseType.UpdateDetals(null);
        expenseType.Details.Should().BeNull();
    }

    [Test]
    public void 領収書フラグが更新されること()
    {
        var expenseType = new ExpenseType(new ExpenseTypeName("経費種別名"), "説明", false);
        expenseType.IsReceipt.Should().BeFalse();
        expenseType.UpdateIsReceipt(true);
        expenseType.IsReceipt.Should().BeTrue();
        expenseType.UpdateIsReceipt(false);
        expenseType.IsReceipt.Should().BeFalse();
    }

    [Test]
    public void 有効フラグが更新されること()
    {
        var expenseType = new ExpenseType(new ExpenseTypeId(), new ExpenseTypeName("経費種別名"), "説明", false, false);
        expenseType.IsActive.Should().BeFalse();
        expenseType.UpdateIsActive(true);
        expenseType.IsActive.Should().BeTrue();
        expenseType.UpdateIsActive(false);
        expenseType.IsActive.Should().BeFalse();
    }
}