namespace DbMigration.Tables;

/// <summary>区分テーブル</summary>
public class Category
{
    /// <summary>ID</summary>
    public Ulid Id { get; set; }

    /// <summary>区分名</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>説明</summary>
    public string? Details { get; set; }

    /// <summary>領収書フラグ</summary>
    public bool IsReceipt { get; set; }

    /// <summary>有効フラグ</summary>
    public bool IsActive { get; set; }
}