namespace Domain.Models.TraveringInformations;

public record EndSection
{
    private string value = string.Empty;

    public EndSection(string value)
    {
        Value = value;
    }

    public string Value
    {
        get => value;
        private set
        {
            DomainException.ThrowIfEmpty(value, "到着地");
            this.value = value.Trim();
        }
    }

    public override string ToString() => Value;
}