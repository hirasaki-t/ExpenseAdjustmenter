using Domain;
using Domain.Models.Expenses;
using Domain.Models.SundryInformations;
using Domain.Repositories;
using Domain.Storages;
using Moq;
using Usecase.SundryExpenses;

namespace UnitTest.Usecase.SundryExpenses;

public class SundryExpenseDeleterTest
{
    [Test]
    public async Task 諸経費精算の削除に成功すること()
    {
        var sundryInformationRepository = new Mock<ISundryInformationRepository>();
        var expenseRepository = new Mock<IExpenseRepository>();
        var approveHistoryRepository = new Mock<IApproveHistoryRepository>();
        var storage = new Mock<IStorage>();
        var id = Ulid.NewUlid().ToString();
        var sundryInformation = new SundryInformation(new(), null, new(1));
        var expense = new Expense(new(), new(2023, 1, 8), new(4000), SubmissionMethod.Electronics, new(), sundryInformation);

        expenseRepository.Setup(x => x.GetAsync(new ExpenseId(id))).ReturnsAsync(expense);

        await new SundryExpenseDeleter(sundryInformationRepository.Object, expenseRepository.Object, approveHistoryRepository.Object, storage.Object)
            .DeleteAsync(id);

        approveHistoryRepository.Verify(x => x.DeleteRangeAsync(new(id)));
        storage.Verify(x => x.DeleteAsync("領収書", expense.ReceiptId!.ToString()));
        sundryInformationRepository.Verify(x => x.DeleteAsync(new(id)));
        expenseRepository.Verify(x => x.DeleteAsync(new(id)));
        sundryInformationRepository.Verify(x => x.CommitAsync());
        expenseRepository.Verify(x => x.CommitAsync());
    }

    [Test]
    public async Task 経費精算が見つからない場合は例外になること()
    {
        var expenseRepository = new Mock<IExpenseRepository>();
        var id = Ulid.NewUlid().ToString();
        var sundryInformation = new SundryInformation(new(), null, new(1));
        var expense = new Expense(new(), new(2023, 1, 8), new(4000), SubmissionMethod.Electronics, new(), sundryInformation);

        expenseRepository.Setup(x => x.GetAsync(It.IsAny<ExpenseId>())).ReturnsAsync((Expense?)null);

        await FluentActions.Invoking(() => new SundryExpenseDeleter(
            new Mock<ISundryInformationRepository>().Object,
            expenseRepository.Object,
            new Mock<IApproveHistoryRepository>().Object,
            new Mock<IStorage>().Object).DeleteAsync(id))
                .Should().ThrowAsync<DomainException>().WithMessage("対象の経費精算が存在しません。");
    }

    [Test]
    public async Task 領収書がNullの場合はファイル削除処理は呼ばれないこと()
    {
        var sundryInformationRepository = new Mock<ISundryInformationRepository>();
        var expenseRepository = new Mock<IExpenseRepository>();
        var approveHistoryRepository = new Mock<IApproveHistoryRepository>();
        var storage = new Mock<IStorage>();
        var id = Ulid.NewUlid().ToString();
        var sundryInformation = new SundryInformation(new(), null, new(1));
        var expense = new Expense(new(), new(2023, 1, 8), new(4000), SubmissionMethod.Bringing, null, sundryInformation);

        expenseRepository.Setup(x => x.GetAsync(new ExpenseId(id))).ReturnsAsync(expense);

        await new SundryExpenseDeleter(sundryInformationRepository.Object, expenseRepository.Object, approveHistoryRepository.Object, storage.Object)
            .DeleteAsync(id);

        approveHistoryRepository.Verify(x => x.DeleteRangeAsync(new(id)));
        storage.Invoking(x => x.Verify(x => x.DeleteAsync(It.IsAny<string>(), It.IsAny<string>()))).Should().Throw<MockException>();
        sundryInformationRepository.Verify(x => x.DeleteAsync(new(id)));
        expenseRepository.Verify(x => x.DeleteAsync(new(id)));
        sundryInformationRepository.Verify(x => x.CommitAsync());
        expenseRepository.Verify(x => x.CommitAsync());
    }
}