namespace Domain.Models.TraveringInformations;

public record WorkName
{
    private string value = string.Empty;

    private static readonly string[] registeredValues = new[] { "社内業務", "営業", "現場業務" };

    public WorkName(string value)
    {
        Value = value;
    }

    public string Value
    {
        get => value;
        private set
        {
            DomainException.ThrowIfEmpty(value, "業務名");
            value = value.Trim();
            if (!registeredValues.Contains(value))
                throw new DomainException("有効な業務名を指定してください。");
            this.value = value;
        }
    }

    public override string ToString() => Value;

    public static WorkName InternalWork => new("社内業務");

    public static WorkName Sales => new("営業");

    public static WorkName FieldWork => new("現場業務");
}