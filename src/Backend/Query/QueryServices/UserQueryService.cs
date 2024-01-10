using System.Data;
using Dapper;
using Query.Datas;

namespace Query.QueryServices;

public class UserQueryService : QueryServiceBase
{
    private const string GetAllSQL = @"
SELECT
    Id,
    Name,
    Mail,
    IsAdmin,
    IsActive
FROM
    Users
";

    private const string GetByIdSQL = GetAllSQL + @"
WHERE
    Id = @Id";

    public UserQueryService(IDbConnection connection) : base(connection)
    {
    }

    public Task<IEnumerable<UserData>> GetAllAsync() => connection.QueryAsync<UserData>(GetAllSQL);

    public Task<UserData?> GetAsync(string id) =>
        connection.QuerySingleOrDefaultAsync<UserData?>(GetByIdSQL, new { Id = id });
}