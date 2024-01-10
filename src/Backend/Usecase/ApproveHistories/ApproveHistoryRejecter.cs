using Domain;
using Domain.Models.ApproveHistories;
using Domain.Models.Users;
using Domain.Repositories;
using Domain.Services;

namespace Usecase.ApproveHistories;

public class ApproveHistoryRejecter
{
    private readonly IApproveHistoryRepository approveHistoryRepository;
    private readonly IUserRepository userRepository;
    private readonly ILoginUserGetter loginUserGetter;

    public ApproveHistoryRejecter(
        IApproveHistoryRepository approveHistoryRepository,
        IUserRepository userRepository,
        ILoginUserGetter loginUserGetter)
    {
        this.approveHistoryRepository = approveHistoryRepository;
        this.userRepository = userRepository;
        this.loginUserGetter = loginUserGetter;
    }

    public async Task RejectAsync(string[] expenseIds, string? comment)
    {
        var mail = await loginUserGetter.GetMailAsync();
        var user = await userRepository.GetAsync(new Mail(mail));
        DomainException.ThrowIfNotFound(user, "ユーザー");
        if (!user.IsAdmin) throw new DomainException("管理者権限がありません。");

        // TODO:Teams or メール自動投稿

        var approveHistoires = expenseIds.Select(x => new ApproveHistory(new(x), user.Id, comment, ApproveStatus.Reject));
        await approveHistoryRepository.AddRangeAsync(approveHistoires);
        await approveHistoryRepository.CommitAsync();
    }
}