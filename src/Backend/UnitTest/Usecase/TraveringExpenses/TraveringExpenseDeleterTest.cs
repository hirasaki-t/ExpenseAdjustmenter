using Domain;
using Domain.Models.Expenses;
using Domain.Models.TraveringInformations;
using Domain.Repositories;
using Domain.Storages;
using Moq;
using Usecase.TraveringExpenses;

namespace UnitTest.Usecase.TraveringExpenses;

public class TraveringExpenseDeleterTest
{
    [Test]
    public async Task 旅費交通費精算の削除に成功すること()
    {
        var traveringInformationRepository = new Mock<ITraveringInformationRepository>();
        var expenseRepository = new Mock<IExpenseRepository>();
        var approveHistoryRepository = new Mock<IApproveHistoryRepository>();
        var storage = new Mock<IStorage>();
        var id = Ulid.NewUlid().ToString();
        var traveringInformation = new TraveringInformation(new(), null, null, WorkName.FieldWork, null);
        var expense = new Expense(new(), new(2023, 1, 8), new(9000), SubmissionMethod.Electronics, new(), traveringInformation);

        expenseRepository.Setup(x => x.GetAsync(new ExpenseId(id))).ReturnsAsync(expense);

        await new TraveringExpenseDeleter(traveringInformationRepository.Object, expenseRepository.Object, approveHistoryRepository.Object, storage.Object)
            .DeleteAsync(id);

        approveHistoryRepository.Verify(x => x.DeleteRangeAsync(new(id)));
        storage.Verify(x => x.DeleteAsync("領収書", expense.ReceiptId!.ToString()));
        traveringInformationRepository.Verify(x => x.DeleteAsync(new(id)));
        expenseRepository.Verify(x => x.DeleteAsync(new(id)));
        traveringInformationRepository.Verify(x => x.CommitAsync());
        expenseRepository.Verify(x => x.CommitAsync());
    }

    [Test]
    public async Task 経費精算が見つからない場合は例外になること()
    {
        var traveringInformationRepository = new Mock<ITraveringInformationRepository>();
        var expenseRepository = new Mock<IExpenseRepository>();
        var storage = new Mock<IStorage>();
        var id = Ulid.NewUlid().ToString();
        var traveringInformation = new TraveringInformation(new(), null, null, WorkName.FieldWork, null);
        var expense = new Expense(new(), new(2023, 1, 8), new(9000), SubmissionMethod.Electronics, new(), traveringInformation);

        expenseRepository.Setup(x => x.GetAsync(It.IsAny<ExpenseId>())).ReturnsAsync((Expense?)null);

        await FluentActions.Invoking(() => new TraveringExpenseDeleter(
            traveringInformationRepository.Object,
            expenseRepository.Object,
            new Mock<IApproveHistoryRepository>().Object,
            storage.Object).DeleteAsync(id))
                .Should().ThrowAsync<DomainException>().WithMessage("対象の経費精算が存在しません。");
    }

    [Test]
    public async Task 領収書がNullの場合はファイル削除処理は呼ばれないこと()
    {
        var traveringInformationRepository = new Mock<ITraveringInformationRepository>();
        var expenseRepository = new Mock<IExpenseRepository>();
        var approveHistoryRepository = new Mock<IApproveHistoryRepository>();
        var storage = new Mock<IStorage>();
        var id = Ulid.NewUlid().ToString();
        var traveringInformation = new TraveringInformation(new(), null, null, WorkName.FieldWork, null);
        var expense = new Expense(new(), new(2023, 1, 8), new(9000), SubmissionMethod.Mailing, null, traveringInformation);

        expenseRepository.Setup(x => x.GetAsync(new ExpenseId(id))).ReturnsAsync(expense);

        await new TraveringExpenseDeleter(traveringInformationRepository.Object, expenseRepository.Object, approveHistoryRepository.Object, storage.Object)
            .DeleteAsync(id);

        approveHistoryRepository.Verify(x => x.DeleteRangeAsync(new(id)));
        storage.Invoking(x => x.Verify(x => x.DeleteAsync(It.IsAny<string>(), It.IsAny<string>()))).Should().Throw<MockException>();
        traveringInformationRepository.Verify(x => x.DeleteAsync(new(id)));
        expenseRepository.Verify(x => x.DeleteAsync(new(id)));
        traveringInformationRepository.Verify(x => x.CommitAsync());
        expenseRepository.Verify(x => x.CommitAsync());
    }
}