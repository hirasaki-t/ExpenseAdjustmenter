using Domain.Models.Categories;
using Domain.Models.Expenses;

namespace Domain.Models.TraveringInformations;

public class TraveringInformation
{
    public TraveringInformation(CategoryId categoryId, StartSection? startSection, EndSection? endSection, WorkName workName, string? remarks) :
        this(new(), categoryId, startSection, endSection, workName, remarks) { }

    public TraveringInformation(ExpenseId expenseId, CategoryId categoryId, StartSection? startSection, EndSection? endSection, WorkName workName, string? remarks)
    {
        ExpenseId = expenseId;
        CategoryId = categoryId;
        StartSection = startSection;
        EndSection = endSection;
        WorkName = workName;
        Remarks = remarks;
    }

    public ExpenseId ExpenseId { get; }

    public CategoryId CategoryId { get; private set; }

    public StartSection? StartSection { get; private set; }

    public EndSection? EndSection { get; private set; }

    public WorkName WorkName { get; private set; }

    public string? Remarks { get; private set; }

    public void Update(CategoryId categoryId, StartSection? startSection, EndSection? endSection, WorkName workName, string? remarks)
    {
        CategoryId = categoryId;
        StartSection = startSection;
        EndSection = endSection;
        WorkName = workName;
        Remarks = remarks;
    }
}