namespace DbMigration.Tables;

/// <summary>承認履歴テーブル</summary>
public class ApproveHistory
{
    /// <summary>ID</summary>
    public Ulid Id { get; set; }

    /// <summary>経費精算ID</summary>
    public Ulid ExpenseId { get; set; }

    /// <summary>ユーザーID</summary>
    public Ulid UserId { get; set; }

    /// <summary>日付</summary>
    public DateTime Date { get; set; }

    /// <summary>コメント</summary>
    public string? Comment { get; set; }

    /// <summary>ステータス</summary>
    public string Status { get; set; } = string.Empty;
}