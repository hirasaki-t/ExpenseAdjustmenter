namespace Domain.Models.Categories;

public record CategoryName
{
    private string value = string.Empty;

    public CategoryName(string value)
    {
        Value = value;
    }

    public string Value
    {
        get => value;
        private set
        {
            DomainException.ThrowIfEmpty(value, "交通区分名");
            this.value = value.Trim();
        }
    }

    public override string ToString() => Value;
}