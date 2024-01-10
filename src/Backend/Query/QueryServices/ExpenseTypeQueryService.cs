using System.Data;
using Dapper;
using Query.Datas;

namespace Query.QueryServices;

public class ExpenseTypeQueryService : QueryServiceBase
{
    private const string GetAllSQL = @"
SELECT
    Id,
    Name,
    Details,
    IsReceipt,
    IsActive
FROM
    ExpenseTypes
";

    private const string GetByIdSQL = GetAllSQL + @"
WHERE
    Id = @Id";

    public ExpenseTypeQueryService(IDbConnection connection) : base(connection)
    {
    }

    public Task<IEnumerable<ExpenseTypeData>> GetAllAsync() => connection.QueryAsync<ExpenseTypeData>(GetAllSQL);

    public Task<ExpenseTypeData?> GetAsync(string id) =>
        connection.QuerySingleOrDefaultAsync<ExpenseTypeData?>(GetByIdSQL, new { Id = id });
}