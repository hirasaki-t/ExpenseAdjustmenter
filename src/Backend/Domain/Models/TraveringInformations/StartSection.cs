namespace Domain.Models.TraveringInformations;

public record StartSection
{
    private string value = string.Empty;

    public StartSection(string value)
    {
        Value = value;
    }

    public string Value
    {
        get => value;
        private set
        {
            DomainException.ThrowIfEmpty(value, "出発地");
            this.value = value.Trim();
        }
    }

    public override string ToString() => Value;
}