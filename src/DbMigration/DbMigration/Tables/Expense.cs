namespace DbMigration.Tables;

/// <summary>経費精算テーブル</summary>
public class Expense
{
    /// <summary>ID</summary>
    public Ulid Id { get; set; }

    /// <summary>ユーザーID</summary>
    public Ulid UserId { get; set; }

    /// <summary>日付</summary>
    public DateOnly Date { get; set; }

    /// <summary>金額</summary>
    public int Amount { get; set; }

    /// <summary>領収書提出方法</summary>
    public string? SubmissionMethod { get; set; }

    /// <summary>領収書ID</summary>
    public Ulid? ReceiptId { get; set; }
}