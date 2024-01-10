using Domain;
using Domain.Models.Expenses;
using Domain.Models.SundryInformations;
using Domain.Models.Users;
using Domain.Repositories;
using Domain.Services;
using Moq;
using Usecase.ApproveHistories;

namespace UnitTest.Usecase.ApproveHistories;

public class ApproveHistoryApplicationerTest
{
    [Test]
    public async Task ユーザーの取得に失敗した場合に例外になること()
    {
        var userRepository = new Mock<IUserRepository>();
        var loginUserGetter = new Mock<ILoginUserGetter>();
        var user = new User(new("山田 太郎"), new("yamada@google.com"), true, true);

        loginUserGetter.Setup(x => x.GetMailAsync()).ReturnsAsync("yamada@google.com");
        userRepository.Setup(x => x.GetAsync(user.Mail)).ReturnsAsync((User?)null);

        await FluentActions.Invoking(() => new ApproveHistoryApplicationer(new Mock<IApproveHistoryRepository>().Object, new Mock<IExpenseRepository>().Object, userRepository.Object, loginUserGetter.Object)
            .ApplicationAsync(new DateOnly()))
            .Should().ThrowAsync<DomainException>().WithMessage("対象のユーザーが存在しません。");
    }

    [Test]
    public async Task 申請履歴の追加に成功すること()
    {
        var approveHistoryRepository = new Mock<IApproveHistoryRepository>();
        var expenseRepository = new Mock<IExpenseRepository>();
        var userRepository = new Mock<IUserRepository>();
        var loginUserGetter = new Mock<ILoginUserGetter>();
        var user = new User(new("山田 太郎"), new("yamada@google.com"), true, true);
        var sundryInformation = new SundryInformation(new(), null, new(5));
        var expenses = new[] { new Expense(user.Id, new DateOnly(2022, 12, 25), new(500), null, null, sundryInformation) };

        loginUserGetter.Setup(x => x.GetMailAsync()).ReturnsAsync("yamada@google.com");
        userRepository.Setup(x => x.GetAsync(user.Mail)).ReturnsAsync(user);
        expenseRepository.Setup(x => x.GetsAsync(It.IsAny<UserId>(), It.IsAny<DateOnly>())).ReturnsAsync(expenses);

        await new ApproveHistoryApplicationer(approveHistoryRepository.Object, expenseRepository.Object, userRepository.Object, loginUserGetter.Object)
            .ApplicationAsync(new DateOnly());

        approveHistoryRepository.Verify(x => x.CommitAsync());
    }
}