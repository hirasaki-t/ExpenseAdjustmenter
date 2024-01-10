using System.Data;
using Dapper;
using Query.Datas;

namespace Query.QueryServices;

public class SundryExpenseQueryService : QueryServiceBase
{
    private const string GetAllSQL = @"
SELECT
    Expenses.Id,
    Expenses.[Date],
    ExpenseTypeId,
    Details,
    ParticipationNumber,
    Amount,
    ReceiptId,
    SubmissionMethod,
    ApproveHistories.UserId AS ReviewerId,
    Comment,
    [Status]
FROM
    Expenses
INNER JOIN
    SundryInformations
ON
    Id = SundryInformations.ExpenseId
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
INNER JOIN
    Users
ON
    Expenses.UserId = Users.Id
WHERE
    Users.Mail = @Mail
";

    private const string GetBySelectedDatesSQL = GetAllSQL + @"
AND
    Expenses.Date BETWEEN @StartDate AND @EndDate
";

    private const string GetByIdSQL = GetAllSQL + @"
AND
    Expenses.Id = @Id
";

    private const string GetSelectableMonthsSQL = @"
SELECT DISTINCT
    CONVERT(DATETIME, FORMAT(Date, 'yyyyMM01'))
FROM
    Expenses
INNER JOIN
    SundryInformations
ON
    Id = SundryInformations.ExpenseId
INNER JOIN
    Users
ON
    UserId = Users.Id
WHERE
    Users.Mail = @Mail
";

    public SundryExpenseQueryService(IDbConnection connection) : base(connection)
    {
    }

    public Task<IEnumerable<SundryExpenseData>> GetMonthDatasAsync(DateOnly date, string mail) =>
        connection.QueryAsync<SundryExpenseData>(GetBySelectedDatesSQL, new { StartDate = new DateOnly(date.Year, date.Month, 1), EndDate = new DateOnly(date.AddMonths(1).Year, date.AddMonths(1).Month, 1).AddDays(-1), Mail = mail });

    public Task<SundryExpenseData?> GetAsync(string id, string mail) =>
        connection.QuerySingleOrDefaultAsync<SundryExpenseData?>(GetByIdSQL, new { Id = id, Mail = mail });

    public Task<IEnumerable<DateOnly>> GetSelectableMonthsAsync(string mail) =>
        connection.QueryAsync<DateOnly>(GetSelectableMonthsSQL, new { Mail = mail });

}