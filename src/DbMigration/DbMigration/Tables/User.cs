namespace DbMigration.Tables;

/// <summary>ユーザーテーブル</summary>
public class User
{
    /// <summary>ID</summary>
    public Ulid Id { get; set; }

    /// <summary>氏名</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>メールアドレス</summary>
    public string Mail { get; set; } = string.Empty;

    /// <summary>管理者フラグ</summary>
    public bool IsAdmin { get; set; }

    /// <summary>有効フラグ</summary>
    public bool IsActive { get; set; }
}