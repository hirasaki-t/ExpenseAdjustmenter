using Domain.Models.ApproveHistories;
using Domain.Models.Expenses;
using Domain.Models.Users;

namespace UnitTest.Domain.Models.ApproveHistories;

public class ApproveHistoryTest
{
    [Test]
    public void 正しい情報で生成される()
    {
        var expenseId = new ExpenseId();
        var userId = new UserId();
        var approveStatus = ApproveStatus.Approve;
        var approveHistory = new ApproveHistory(expenseId, userId, "承認します。", approveStatus);

        approveHistory.Should().BeEquivalentTo(new
        {
            ExpenseId = expenseId,
            UserId = userId,
            Comment = "承認します。",
            Status = approveStatus
        });
    }
}