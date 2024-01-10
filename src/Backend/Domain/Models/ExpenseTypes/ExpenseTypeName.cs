namespace Domain.Models.ExpenseTypes;

public record ExpenseTypeName
{
    private string value = string.Empty;

    public ExpenseTypeName(string value)
    {
        Value = value;
    }

    public string Value
    {
        get => value;
        private set
        {
            DomainException.ThrowIfEmpty(value, "経費種別名");
            this.value = value.Trim();
        }
    }

    public override string ToString() => Value;
}