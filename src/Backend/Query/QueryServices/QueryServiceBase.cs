using System.Data;

namespace Query.QueryServices;

public abstract class QueryServiceBase
{
    protected IDbConnection connection;

    protected QueryServiceBase(IDbConnection connection)
    {
        this.connection = connection;
    }
}