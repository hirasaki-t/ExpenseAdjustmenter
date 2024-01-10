﻿using Domain;
using Domain.Models.ApproveHistories;
using Domain.Models.Users;
using Domain.Repositories;
using Domain.Services;
using Moq;
using Usecase.ApproveHistories;

namespace UnitTest.Usecase.ApproveHistories;

public class ApproveHistoryRejecterTest
{
    [Test]
    public async Task ユーザーの取得に失敗した場合に例外になること()
    {
        var userRepository = new Mock<IUserRepository>();
        var loginUserGetter = new Mock<ILoginUserGetter>();
        var user = new User(new("山田 太郎"), new("yamada@google.com"), true, true);

        loginUserGetter.Setup(x => x.GetMailAsync()).ReturnsAsync("yamada@google.com");
        userRepository.Setup(x => x.GetAsync(user.Mail)).ReturnsAsync((User?)null);

        await FluentActions.Invoking(() => new ApproveHistoryRejecter(new Mock<IApproveHistoryRepository>().Object, userRepository.Object, loginUserGetter.Object)
            .RejectAsync(new[] { Ulid.NewUlid().ToString() }, null))
            .Should().ThrowAsync<DomainException>().WithMessage("対象のユーザーが存在しません。");
    }

    [Test]
    public async Task 実行者が管理者でない場合に例外になること()
    {
        var userRepository = new Mock<IUserRepository>();
        var loginUserGetter = new Mock<ILoginUserGetter>();
        var user = new User(new("山田 太郎"), new("yamada@google.com"), false, true);

        loginUserGetter.Setup(x => x.GetMailAsync()).ReturnsAsync("yamada@google.com");
        userRepository.Setup(x => x.GetAsync(user.Mail)).ReturnsAsync(user);

        await FluentActions.Invoking(() => new ApproveHistoryRejecter(new Mock<IApproveHistoryRepository>().Object, userRepository.Object, loginUserGetter.Object)
            .RejectAsync(new[] { Ulid.NewUlid().ToString() }, null))
            .Should().ThrowAsync<DomainException>().WithMessage("管理者権限がありません。");
    }

    [Test]
    public async Task 経費精算の申請の否認に成功すること()
    {
        var approveHistoryRepository = new Mock<IApproveHistoryRepository>();
        var userRepository = new Mock<IUserRepository>();
        var loginUserGetter = new Mock<ILoginUserGetter>();
        var user = new User(new("山田 太郎"), new("yamada@google.com"), true, true);
        var approveHistory = new ApproveHistory(new(), user.Id, "否認します", ApproveStatus.Reject);

        loginUserGetter.Setup(x => x.GetMailAsync()).ReturnsAsync("yamada@google.com");
        userRepository.Setup(x => x.GetAsync(user.Mail)).ReturnsAsync(user);

        await new ApproveHistoryRejecter(approveHistoryRepository.Object, userRepository.Object, loginUserGetter.Object)
            .RejectAsync(new[] { approveHistory.ExpenseId.ToString() }, "否認します");

        approveHistoryRepository.Verify(x => x.CommitAsync());
    }
}