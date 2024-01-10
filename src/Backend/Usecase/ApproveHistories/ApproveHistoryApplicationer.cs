using Domain;
using Domain.Models.ApproveHistories;
using Domain.Models.Expenses;
using Domain.Models.Users;
using Domain.Repositories;
using Domain.Services;

namespace Usecase.ApproveHistories;

public class ApproveHistoryApplicationer
{
    private readonly IApproveHistoryRepository approveHistoryRepository;
    private readonly IExpenseRepository expenseRepository;
    private readonly IUserRepository userRepository;
    private readonly ILoginUserGetter loginUserGetter;

    public ApproveHistoryApplicationer(
        IApproveHistoryRepository approveHistoryRepository,
        IExpenseRepository expenseRepository,
        IUserRepository userRepository,
        ILoginUserGetter loginUserGetter)
    {
        this.approveHistoryRepository = approveHistoryRepository;
        this.expenseRepository = expenseRepository;
        this.userRepository = userRepository;
        this.loginUserGetter = loginUserGetter;
    }

    public async Task ApplicationAsync(DateOnly date)
    {
        var mail = await loginUserGetter.GetMailAsync();
        var user = await userRepository.GetAsync(new Mail(mail));
        DomainException.ThrowIfNotFound(user, "ユーザー");

        var dictionary = await approveHistoryRepository.GetLatestStatusDictionaryAsync();
        var expenses = await expenseRepository.GetsAsync(user.Id, date);
        var approveHistories = expenses
            .Where(x => !dictionary.TryGetValue(x.Id, out var approveStatus) || approveStatus == ApproveStatus.Reject)
            .Select(x => new ApproveHistory(x.Id, user.Id, null, ApproveStatus.Application));
        await approveHistoryRepository.AddRangeAsync(approveHistories);
        await approveHistoryRepository.CommitAsync();
    }
}