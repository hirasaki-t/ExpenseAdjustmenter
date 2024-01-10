using Domain;
using Domain.Models.Expenses;
using Domain.Models.TraveringInformations;
using Domain.Models.Users;
using Domain.Repositories;
using Domain.Services;
using Domain.Storages;
using Moq;
using Usecase.TraveringExpenses;

namespace UnitTest.Usecase.TraveringExpenses;

public class TraveringExpenseUpdaterTest
{
    [Test]
    public async Task 旅費交通費情報の取得に失敗した場合に例外になること()
    {
        var traveringInformationRepository = new Mock<ITraveringInformationRepository>();
        var user = new User(new("山田 太郎"), new("yamada@google.com"), true, true);

        traveringInformationRepository.Setup(x => x.GetAsync(It.IsAny<ExpenseId>())).ReturnsAsync((TraveringInformation?)null);

        await FluentActions.Invoking(() => new TraveringExpenseUpdater(
            traveringInformationRepository.Object,
            new Mock<IExpenseRepository>().Object,
            new Mock<IStorage>().Object).UpdateAsync(Ulid.NewUlid().ToString(), new DateOnly(), "", null, null, "", null, null, 0, null))
            .Should().ThrowAsync<DomainException>().WithMessage("対象の旅費・交通費情報が存在しません。");
    }

    [Test]
    public async Task 経費精算の取得に失敗した場合に例外になること()
    {
        var traveringInformationRepository = new Mock<ITraveringInformationRepository>();
        var expenseRepository = new Mock<IExpenseRepository>();
        var user = new User(new("山田 太郎"), new("yamada@google.com"), true, true);
        var traveringInformation = new TraveringInformation(new(), null, null, WorkName.FieldWork, null);
        var id = Ulid.NewUlid().ToString();

        traveringInformationRepository.Setup(x => x.GetAsync(new ExpenseId(id))).ReturnsAsync(traveringInformation);
        expenseRepository.Setup(x => x.GetAsync(It.IsAny<ExpenseId>())).ReturnsAsync((Expense?)null);

        await FluentActions.Invoking(() => new TraveringExpenseUpdater(
            traveringInformationRepository.Object,
            expenseRepository.Object,
            new Mock<IStorage>().Object).UpdateAsync(id, new DateOnly(), "", null, null, "", null, null, 0, null))
            .Should().ThrowAsync<DomainException>().WithMessage("対象の経費精算が存在しません。");
    }

    [Test]
    public async Task 旅費交通費精算の更新に成功すること()
    {
        var traveringInformationRepository = new Mock<ITraveringInformationRepository>();
        var expenseRepository = new Mock<IExpenseRepository>();
        var user = new User(new("山田 太郎"), new("yamada@google.com"), true, true);
        var traveringInformation = new TraveringInformation(new(), null, null, WorkName.FieldWork, null);
        var expense = new Expense(new(), new DateOnly(2022, 12, 22), new(500), null, null, traveringInformation);
        var id = Ulid.NewUlid().ToString();

        traveringInformationRepository.Setup(x => x.GetAsync(new ExpenseId(id))).ReturnsAsync(traveringInformation);
        expenseRepository.Setup(x => x.GetAsync(new ExpenseId(id))).ReturnsAsync(expense);

        await new TraveringExpenseUpdater(
            traveringInformationRepository.Object,
            expenseRepository.Object,
            new Mock<IStorage>().Object).UpdateAsync(id, new DateOnly(), "営業", null, null, Ulid.NewUlid().ToString(), null, null, 0, null);

        expenseRepository.Verify(x => x.UpdateAsync(expense));
        traveringInformationRepository.Verify(x => x.UpdateAsync(traveringInformation));
        expenseRepository.Verify(x => x.CommitAsync());
        traveringInformationRepository.Verify(x => x.CommitAsync());
    }
}