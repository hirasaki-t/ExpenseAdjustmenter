namespace DbMigration.Tables;

/// <summary>諸経費情報テーブル</summary>
public class SundryInformation
{
    /// <summary>経費精算ID</summary>
    public Ulid ExpenseId { get; set; }

    /// <summary>経費種別ID</summary>
    public Ulid ExpenseTypeId { get; set; }

    /// <summary>詳細</summary>
    public string? Details { get; set; }

    /// <summary>人数</summary>
    public int ParticipationNumber { get; set; }
}