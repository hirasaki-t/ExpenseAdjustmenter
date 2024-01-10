using Domain;
using Domain.Models.ExpenseTypes;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.Repositories.EFCore;
using Moq;
using Usecase.ExpenseTypes;
using Usecase.Users;

namespace UnitTest.Usecase.ExpenseTypes;

public class ExpenseTypeUpdaterTest
{
    [Test]
    public async Task 経費種別の更新に成功すること()
    {
        var expenseTypeRepository = new Mock<IExpenseTypeRepository>();
        var expenseTypeDuplicateChecker = new Mock<IExpenseTypeDuplicateChecker>();
        var expenseType = new ExpenseType(new ExpenseTypeName("会議費"), "5000円以下", true);

        expenseTypeRepository.Setup(x => x.GetAsync(It.IsAny<ExpenseTypeId>())).ReturnsAsync(expenseType);
        expenseTypeDuplicateChecker.Setup(x => x.DuplicatedAsync(It.IsAny<ExpenseType>())).ReturnsAsync(false);

        await new ExpenseTypeUpdater(expenseTypeRepository.Object, expenseTypeDuplicateChecker.Object).UpdateAsync(Ulid.NewUlid().ToString(), "接待費", "5000円以上", false, true);

        expenseTypeRepository.Verify(x => x.UpdateAsync(expenseType));
        expenseTypeRepository.Verify(x => x.CommitAsync());
    }

    [Test]
    public async Task 経費種別IDから経費種別が取得できない場合は例外になること()
    {
        var expenseTypeRepository = new Mock<IExpenseTypeRepository>();

        expenseTypeRepository.Setup(x => x.GetAsync(It.IsAny<ExpenseTypeId>())).ReturnsAsync((ExpenseType?)null);

        await FluentActions.Invoking(() => new ExpenseTypeUpdater(expenseTypeRepository.Object, new Mock<IExpenseTypeDuplicateChecker>().Object).UpdateAsync(Ulid.NewUlid().ToString(), "雑費", null, true, true))
           .Should().ThrowAsync<DomainException>().WithMessage("対象の経費種別が存在しません。");
    }

    [Test]
    public async Task 重複する経費種別名に更新しようとすると例外になること()
    {
        var expenseTypeRepository = new Mock<IExpenseTypeRepository>();
        var expenseTypeDuplicateChecker = new Mock<IExpenseTypeDuplicateChecker>();
        var expenseType = new ExpenseType(new ExpenseTypeName("会議費"), "5000円以下", true);

        expenseTypeRepository.Setup(x => x.GetAsync(It.IsAny<ExpenseTypeId>())).ReturnsAsync(expenseType);
        expenseTypeDuplicateChecker.Setup(x => x.DuplicatedAsync(It.IsAny<ExpenseType>())).ReturnsAsync(true);

        await FluentActions.Invoking(() => new ExpenseTypeUpdater(expenseTypeRepository.Object, expenseTypeDuplicateChecker.Object).UpdateAsync(Ulid.NewUlid().ToString(), "雑費", null, true, true))
           .Should().ThrowAsync<DomainException>().WithMessage("経費種別名[雑費]はすでに登録されています。");
    }
}