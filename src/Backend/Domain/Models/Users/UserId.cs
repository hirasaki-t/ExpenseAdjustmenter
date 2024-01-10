namespace Domain.Models.Users;

public record UserId
{
    public UserId() : this(Ulid.NewUlid())
    {
    }

    public UserId(string value) : this(Ulid.Parse(value))
    {
    }

    public UserId(Ulid value)
    {
        Value = value;
    }

    public Ulid Value { get; }

    public override string ToString()
    {
        return Value.ToString();
    }
}