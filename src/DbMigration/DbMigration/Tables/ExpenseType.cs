namespace DbMigration.Tables;

/// <summary>経費種別テーブル</summary>
public class ExpenseType
{
    /// <summary>ID</summary>
    public Ulid Id { get; set; }

    /// <summary>経費種別名</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>説明</summary>
    public string? Details { get; set; }

    /// <summary>領収書フラグ</summary>
    public bool IsReceipt { get; set; }

    /// <summary>有効フラグ</summary>
    public bool IsActive { get; set; }
}