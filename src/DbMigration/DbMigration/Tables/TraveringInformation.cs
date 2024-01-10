namespace DbMigration.Tables;

/// <summary>交通費情報テーブル</summary>
public class TraveringInformation
{
    /// <summary>経費精算ID</summary>
    public Ulid ExpenseId { get; set; }

    /// <summary>交通区分ID</summary>
    public Ulid CategoryId { get; set; }

    /// <summary>出発地</summary>
    public string? StartSection { get; set; }

    /// <summary>到着地</summary>
    public string? EndSection { get; set; }

    /// <summary>業務名</summary>
    public string WorkName { get; set; } = string.Empty;

    /// <summary>摘要</summary>
    public string? Remarks { get; set; }
}