namespace Domain.Models.Expenses;

public record ReceiptId
{
    public ReceiptId() : this(Ulid.NewUlid())
    {
    }

    public ReceiptId(string value) : this(Ulid.Parse(value))
    {
    }

    public ReceiptId(Ulid value)
    {
        Value = value;
    }

    public Ulid Value { get; }

    public override string ToString()
    {
        return Value.ToString();
    }
}