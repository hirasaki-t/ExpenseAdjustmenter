using static Dapper.SqlMapper;
using System.Data;

namespace Query.Handlers;

/// <summary>DateOnly型のDapper用のコンバーター</summary>
public class DateOnlyHandler : TypeHandler<DateOnly>
{
    public override DateOnly Parse(object value)
    {
        return DateOnly.FromDateTime((DateTime)value);
    }

    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.DbType = DbType.DateTime;
        parameter.Value = value.ToDateTime(TimeOnly.MinValue);
    }
}