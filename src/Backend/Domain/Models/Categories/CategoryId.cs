namespace Domain.Models.Categories;

public record CategoryId
{
    public CategoryId() : this(Ulid.NewUlid())
    {
    }

    public CategoryId(string value) : this(Ulid.Parse(value))
    {
    }

    public CategoryId(Ulid value)
    {
        Value = value;
    }

    public Ulid Value { get; }

    public override string ToString()
    {
        return Value.ToString();
    }
}