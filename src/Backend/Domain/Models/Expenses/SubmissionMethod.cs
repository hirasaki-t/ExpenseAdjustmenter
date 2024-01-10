namespace Domain.Models.Expenses;

public record SubmissionMethod
{
    private string value = string.Empty;

    private static readonly string[] registeredValues = new[] { "紙/持参", "紙/郵送", "電子" };

    public SubmissionMethod(string value)
    {
        Value = value;
    }

    public string Value
    {
        get => value;
        private set
        {
            DomainException.ThrowIfEmpty(value, "提出方法");
            value = value.Trim();
            if (!registeredValues.Contains(value))
                throw new DomainException("有効な提出方法を指定してください。");
            this.value = value;
        }
    }

    public override string ToString() => Value;

    public static SubmissionMethod Bringing { get; } = new("紙/持参");

    public static SubmissionMethod Mailing { get; } = new("紙/郵送");

    public static SubmissionMethod Electronics { get; } = new("電子");
}