namespace Domain.Models.Categories;

public class Category
{
    public Category(CategoryName name, string? details, bool isReceipt) : this(new CategoryId(), name, details, isReceipt, true) { }

    public Category(CategoryId id, CategoryName name, string? details, bool isReceipt, bool isActive)
    {
        Id = id;
        Name = name;
        Details = details;
        IsReceipt = isReceipt;
        IsActive = isActive;
    }

    public CategoryId Id { get; }

    public CategoryName Name { get; private set; }

    public string? Details { get; private set; }

    public bool IsReceipt { get; private set; }

    public bool IsActive { get; private set; }

    public void UpdateName(CategoryName name) => Name = name;

    public void UpdateDetals(string? details) => Details = details;

    public void UpdateIsReceipt(bool isReceipt) => IsReceipt = isReceipt;

    public void UpdateIsActive(bool isActive) => IsActive = isActive;
}