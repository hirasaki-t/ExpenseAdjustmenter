namespace Domain.Models.ApproveHistories;

public record ApproveHistoryId
{
    public ApproveHistoryId() : this(Ulid.NewUlid())
    {
    }

    public ApproveHistoryId(string value) : this(Ulid.Parse(value))
    {
    }

    public ApproveHistoryId(Ulid value)
    {
        Value = value;
    }

    public Ulid Value { get; }

    public override string ToString()
    {
        return Value.ToString();
    }
}