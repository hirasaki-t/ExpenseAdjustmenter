using System.Data;
using Dapper;
using Query.Datas;

namespace Query.QueryServices;

public class CategoryQueryService : QueryServiceBase
{
    private const string GetAllSQL = @"
SELECT
    Id,
    Name,
    Details,
    IsReceipt,
    IsActive
FROM
    Categories
";

    private const string GetByIdSQL = GetAllSQL + @"
WHERE
    Id = @Id";

    public CategoryQueryService(IDbConnection connection) : base(connection)
    {
    }

    public Task<IEnumerable<CategoryData>> GetAllAsync() => connection.QueryAsync<CategoryData>(GetAllSQL);

    public Task<CategoryData?> GetAsync(string id) =>
        connection.QuerySingleOrDefaultAsync<CategoryData?>(GetByIdSQL, new { Id = id });
}