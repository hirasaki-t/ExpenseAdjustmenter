
namespace Domain.Models.ExpenseTypes;

public class ExpenseType
{
    public ExpenseType(ExpenseTypeName name, string? details, bool isReceipt) : this(new ExpenseTypeId(), name, details, isReceipt, true) { }

    public ExpenseType(ExpenseTypeId id, ExpenseTypeName name, string? details, bool isReceipt, bool isActive)
    {
        Id = id;
        Name = name;
        Details = details;
        IsReceipt = isReceipt;
        IsActive = isActive;
    }

    public ExpenseTypeId Id { get; }

    public ExpenseTypeName Name { get; private set; }

    public string? Details { get; private set; }

    public bool IsReceipt { get; private set; }

    public bool IsActive { get; private set; }

    public void UpdateName(ExpenseTypeName name) => Name = name;

    public void UpdateDetals(string? details) => Details = details;

    public void UpdateIsReceipt(bool isReceipt) => IsReceipt = isReceipt;

    public void UpdateIsActive(bool isActive) => IsActive = isActive;
}