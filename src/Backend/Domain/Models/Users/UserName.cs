namespace Domain.Models.Users;

public record UserName
{
    private string value = string.Empty;

    public UserName(string value)
    {
        Value = value;
    }

    public string Value
    {
        get => value;
        private set
        {
            DomainException.ThrowIfEmpty(value, "ユーザー名");
            this.value = value.Trim();
        }
    }

    public override string ToString() => Value;
}