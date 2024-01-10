using Domain;
using Domain.Models.ExpenseTypes;
using Domain.Models.Users;
using Domain.Repositories;
using Domain.Services;
using Moq;
using Usecase.ExpenseTypes;
using Usecase.Users;

namespace UnitTest.Usecase.ExpenseTypes;

public class ExpenseTypeAdderTest
{
    [Test]
    public async Task 経費種別の追加に成功すること()
    {
        var expenseTypeRepository = new Mock<IExpenseTypeRepository>();
        var expenseTypeDuplicateChecker = new Mock<IExpenseTypeDuplicateChecker>();

        expenseTypeDuplicateChecker.Setup(x => x.DuplicatedAsync(It.IsAny<ExpenseType>())).ReturnsAsync(false);

        await new ExpenseTypeAdder(expenseTypeRepository.Object, expenseTypeDuplicateChecker.Object).AddAsync("経費種別名", null, false);

        expenseTypeRepository.Verify(x => x.AddAsync(It.IsAny<ExpenseType>()));
        expenseTypeRepository.Verify(x => x.CommitAsync());
    }

    [Test]
    public async Task 重複する経費種別名を追加しようとすると例外になること()
    {
        var expenseTypeRepository = new Mock<IExpenseTypeDuplicateChecker>();

        expenseTypeRepository.Setup(x => x.DuplicatedAsync(It.IsAny<ExpenseType>())).ReturnsAsync(true);

        await FluentActions.Invoking(() => new ExpenseTypeAdder(new Mock<IExpenseTypeRepository>().Object, expenseTypeRepository.Object).AddAsync("会議費", null, false))
            .Should().ThrowAsync<DomainException>().WithMessage("経費種別名[会議費]はすでに登録されています。");
    }
}