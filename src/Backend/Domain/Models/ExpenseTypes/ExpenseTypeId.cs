namespace Domain.Models.ExpenseTypes;

public record ExpenseTypeId
{
    public ExpenseTypeId() : this(Ulid.NewUlid())
    {
    }

    public ExpenseTypeId(string value) : this(Ulid.Parse(value))
    {
    }

    public ExpenseTypeId(Ulid value)
    {
        Value = value;
    }

    public Ulid Value { get; }

    public override string ToString()
    {
        return Value.ToString();
    }
}