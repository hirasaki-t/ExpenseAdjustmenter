using Domain.Models.Expenses;
using Domain.Models.ExpenseTypes;

namespace Domain.Models.SundryInformations;

public class SundryInformation
{
    public SundryInformation(ExpenseTypeId expenseTypeId, string? details, ParticipationNumber participationNumber)
        :this(new ExpenseId(), expenseTypeId, details, participationNumber) { }

    public SundryInformation(ExpenseId expenseId, ExpenseTypeId expenseTypeId, string? details, ParticipationNumber participationNumber)
    {
        ExpenseId = expenseId;
        ExpenseTypeId = expenseTypeId;
        Details = details;
        ParticipationNumber = participationNumber;
    }

    public ExpenseId ExpenseId { get; }

    public ExpenseTypeId ExpenseTypeId { get; private set; }

    public string? Details { get; private set; }

    public ParticipationNumber ParticipationNumber { get; private set; }

    public void Update(ExpenseTypeId expenseTypeId, string? details, ParticipationNumber participationNumber)
    {
        ExpenseTypeId = expenseTypeId;
        Details = details;
        ParticipationNumber = participationNumber;
    }
}