using System.Data;
using Dapper;
using Query.Datas;

namespace Query.QueryServices;

public class ApproveHistoryQueryService : QueryServiceBase
{
    private const string GetMonthsDataSQL = @"
SELECT
    ApproveHistories.Id,
    Expenses.Id AS ExpenseId,
    Expenses.[Date],
    Expenses.UserId,
    CASE WHEN EXISTS(
        SELECT
            *
        FROM
            TraveringInformations
        WHERE
            ExpenseId = Expenses.Id
    ) THEN N'旅費・交通費' ELSE N'諸経費' END AS Type,
    Amount,
    SubmissionMethod,
    ReceiptId,
    ApproveHistories.UserId As ReviewerId,
    ApproveHistories.Comment,
    ApproveHistories.Status,
    TraveringInformations.WorkName,
    TraveringInformations.StartSection,
    TraveringInformations.EndSection,
    TraveringInformations.CategoryId,
    TraveringInformations.Remarks,
    SundryInformations.ExpenseTypeId,
    SundryInformations.ParticipationNumber,
    SundryInformations.Details
FROM
    Expenses
LEFT OUTER JOIN
    (
        SELECT
            ApproveHistories.*
        FROM
            (
                SELECT
                    ExpenseId,
                    Max(Date) AS Date
                FROM
                    ApproveHistories
                GROUP BY
                    ExpenseId
            ) AS A
            INNER JOIN
                ApproveHistories
            ON
                ApproveHistories.ExpenseId = A.ExpenseId
            AND
                ApproveHistories.[Date] = A.Date
    ) AS ApproveHistories
ON
    Expenses.Id = ApproveHistories.ExpenseId
LEFT OUTER JOIN
    TraveringInformations
ON
    TraveringInformations.ExpenseId = Expenses.Id
LEFT OUTER JOIN
    SundryInformations
ON
    SundryInformations.ExpenseId = Expenses.Id
WHERE
    Status IS NOT NULL
AND
    Expenses.Date BETWEEN @StartDate AND @EndDate
";

    private const string GetSelectableMonthsSQL = @"
SELECT DISTINCT
    CONVERT(DATETIME, FORMAT(Date, 'yyyyMM01'))
FROM
    ApproveHistories
";

    private const string GetApproveHistories = @"
SELECT
    CASE [Status] WHEN N'申請中' THEN N'申請' WHEN N'否認' THEN N'否認' WHEN N'承認' THEN N'承認' END AS Status,
    UserId,
    Comment,
    [Date]
FROM
    ApproveHistories
WHERE
    ExpenseId = @ExpenseId
ORDER BY [Date]
";

    public ApproveHistoryQueryService(IDbConnection connection) : base(connection)
    {
    }

    public Task<IEnumerable<ApproveHistoryData>> GetAsync(DateOnly date) =>
        connection.QueryAsync<ApproveHistoryData>(GetMonthsDataSQL, new { StartDate = new DateOnly(date.Year, date.Month, 1), EndDate = new DateOnly(date.AddMonths(1).Year, date.AddMonths(1).Month, 1).AddDays(-1) });

    public Task<IEnumerable<DateOnly>> GetSelectableMonthsAsync() =>
        connection.QueryAsync<DateOnly>(GetSelectableMonthsSQL);

    public Task<IEnumerable<ApproveHistoryListData>> GetAsync(string expenseId) =>
        connection.QueryAsync<ApproveHistoryListData>(GetApproveHistories, new { ExpenseId = expenseId });
}