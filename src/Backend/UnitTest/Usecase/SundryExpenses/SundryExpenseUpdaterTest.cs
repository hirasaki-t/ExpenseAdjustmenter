using Domain;
using Domain.Models.Expenses;
using Domain.Models.SundryInformations;
using Domain.Models.Users;
using Domain.Repositories;
using Domain.Storages;
using Moq;
using Usecase.SundryExpenses;

namespace UnitTest.Usecase.SundryExpenses;

public class SundryExpenseUpdaterTest
{
    [Test]
    public async Task 諸経費情報の取得に失敗した場合に例外になること()
    {
        var sundryInformationRepository = new Mock<ISundryInformationRepository>();
        var user = new User(new("山田 太郎"), new("yamada@google.com"), true, true);

        sundryInformationRepository.Setup(x => x.GetAsync(It.IsAny<ExpenseId>())).ReturnsAsync((SundryInformation?)null);

        await FluentActions.Invoking(() => new SundryExpenseUpdater(
            sundryInformationRepository.Object,
            new Mock<IExpenseRepository>().Object,
            new Mock<IStorage>().Object).UpdateAsync(Ulid.NewUlid().ToString(), new DateOnly(), "", null, 4, 15000, null, null))
            .Should().ThrowAsync<DomainException>().WithMessage("対象の諸経費情報が存在しません。");
    }

    [Test]
    public async Task 経費精算の取得に失敗した場合に例外になること()
    {
        var sundryInformationRepository = new Mock<ISundryInformationRepository>();
        var expenseRepository = new Mock<IExpenseRepository>();
        var user = new User(new("山田 太郎"), new("yamada@google.com"), true, true);
        var sundryInformation = new SundryInformation(new(), null, new(5));
        var expense = new Expense(new(), new DateOnly(2022, 12, 22), new(500), null, null, sundryInformation);
        var id = Ulid.NewUlid().ToString();

        sundryInformationRepository.Setup(x => x.GetAsync(new ExpenseId(id))).ReturnsAsync(sundryInformation);
        expenseRepository.Setup(x => x.GetAsync(It.IsAny<ExpenseId>())).ReturnsAsync((Expense?)null);

        await FluentActions.Invoking(() => new SundryExpenseUpdater(
            sundryInformationRepository.Object,
            expenseRepository.Object,
            new Mock<IStorage>().Object).UpdateAsync(id, new DateOnly(), "", null, 5, 9000, null, null))
            .Should().ThrowAsync<DomainException>().WithMessage("対象の経費精算が存在しません。");
    }

    [Test]
    public async Task 諸経費精算の更新に成功すること()
    {
        var sundryInformationRepository = new Mock<ISundryInformationRepository>();
        var expenseRepository = new Mock<IExpenseRepository>();
        var user = new User(new("山田 太郎"), new("yamada@google.com"), true, true);
        var sundryInformation = new SundryInformation(new(), null, new(5));
        var expense = new Expense(new(), new DateOnly(2022, 12, 22), new(500), null, null, sundryInformation);
        var id = Ulid.NewUlid().ToString();

        sundryInformationRepository.Setup(x => x.GetAsync(new ExpenseId(id))).ReturnsAsync(sundryInformation);
        expenseRepository.Setup(x => x.GetAsync(new ExpenseId(id))).ReturnsAsync(expense);

        await new SundryExpenseUpdater(
            sundryInformationRepository.Object,
            expenseRepository.Object,
            new Mock<IStorage>().Object).UpdateAsync(id, new DateOnly(2022, 12, 24), Ulid.NewUlid().ToString(), "牛角", 5, 30000, "紙/持参", null);

        expenseRepository.Verify(x => x.UpdateAsync(expense));
        sundryInformationRepository.Verify(x => x.UpdateAsync(sundryInformation));
        expenseRepository.Verify(x => x.CommitAsync());
        sundryInformationRepository.Verify(x => x.CommitAsync());
    }
}