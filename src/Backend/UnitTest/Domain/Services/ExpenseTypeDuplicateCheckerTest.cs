using Domain.Models.ExpenseTypes;
using Domain.Models.Users;
using Domain.Repositories;
using Domain.Services;
using Moq;

namespace UnitTest.Domain.Services;

public class ExpenseTypeDuplicateCheckerTest
{
    [Test]
    public async Task 経費種別名がすでに存在した場合はTRUEが返却されること()
    {
        var expenseTypeRepository = new Mock<IExpenseTypeRepository>();
        var expenseType = new ExpenseType(new ExpenseTypeName("経費種別名"), null, true);

        expenseTypeRepository.Setup(x => x.GetAsync(expenseType.Name)).ReturnsAsync(expenseType);

        (await new ExpenseTypeDuplicateChecker(expenseTypeRepository.Object).DuplicatedAsync(new ExpenseType(new ExpenseTypeName("経費種別名"), null, true))).Should().BeTrue();
    }

    [Test]
    public async Task 経費種別名がまだ存在していない場合はFALSEが返却されること()
    {
        var expenseTypeRepository = new Mock<IExpenseTypeRepository>();
        var expenseType = new ExpenseType(new ExpenseTypeName("経費種別名"), null, true);

        expenseTypeRepository.Setup(x => x.GetAsync(It.IsAny<ExpenseTypeName>())).ReturnsAsync((ExpenseType?)null);

        (await new ExpenseTypeDuplicateChecker(expenseTypeRepository.Object).DuplicatedAsync(expenseType)).Should().BeFalse();
    }
}