namespace Domain.Models.Expenses;

public record ExpenseId
{
    public ExpenseId() : this(Ulid.NewUlid())
    {
    }

    public ExpenseId(string value) : this(Ulid.Parse(value))
    {
    }

    public ExpenseId(Ulid value)
    {
        Value = value;
    }

    public Ulid Value { get; }

    public override string ToString()
    {
        return Value.ToString();
    }
}