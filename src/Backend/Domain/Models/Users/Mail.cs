namespace Domain.Models.Users;

public record Mail
{
    private string value = string.Empty;

    public Mail(string value)
    {
        Value = value;
    }

    public string Value
    {
        get => value;
        private set
        {
            DomainException.ThrowIfEmpty(value, "メールアドレス");
            this.value = value.Trim();
        }
    }

    public override string ToString() => Value;
}