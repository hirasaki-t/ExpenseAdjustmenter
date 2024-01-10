namespace Domain.Models.ApproveHistories;

public record ApproveStatus
{
    private string value = string.Empty;

    private static readonly string[] registeredValues = new[] { "申請中", "承認", "否認", "保留" };

    public ApproveStatus(string value)
    {
        Value = value;
    }

    public string Value
    {
        get => value;
        private set
        {
            DomainException.ThrowIfEmpty(value, "ステータス");
            value = value.Trim();
            if (!registeredValues.Contains(value))
                throw new DomainException("有効なステータスを指定してください。");
            this.value = value;
        }
    }

    public override string ToString() => Value;

    public static ApproveStatus Application { get; } = new("申請中");

    public static ApproveStatus Approve { get; } = new("承認");

    public static ApproveStatus Reject { get; } = new("否認");

    public static ApproveStatus Pending { get; } = new("保留");
}