using Domain.Models.ApproveHistories;
using Domain.Models.SundryInformations;
using Domain.Models.TraveringInformations;
using Domain.Models.Users;

namespace Domain.Models.Expenses;

public class Expense
{
    private Expense(ExpenseId id, UserId userId, DateOnly date, Amount amount, SubmissionMethod? submissionMethod, ReceiptId? receiptId, TraveringInformation? traveringInformation, SundryInformation? sundryInformation)
    {
        Id = id;
        UserId = userId;
        Date = date;
        Amount = amount;
        SubmissionMethod = submissionMethod;
        ReceiptId = receiptId;
        TraveringInformation = traveringInformation;
        SundryInformation = sundryInformation;
    }

    // EFCore用
    private Expense(ExpenseId id, UserId userId, DateOnly date, Amount amount, SubmissionMethod? submissionMethod, ReceiptId? receiptId)
    {
        Id = id;
        UserId = userId;
        Date = date;
        Amount = amount;
        SubmissionMethod = submissionMethod;
        ReceiptId = receiptId;
    }

    public Expense(UserId userId, DateOnly date, Amount amount, SubmissionMethod? submissionMethod, ReceiptId? receiptId, TraveringInformation traveringInformation) :
        this(traveringInformation.ExpenseId, userId, date, amount, submissionMethod, receiptId, traveringInformation, null) { }

    public Expense(UserId userId, DateOnly date, Amount amount, SubmissionMethod? submissionMethod, ReceiptId? receiptId, SundryInformation sundryInformation) :
        this(sundryInformation.ExpenseId, userId, date, amount, submissionMethod, receiptId, null, sundryInformation) { }

    public ExpenseId Id { get; }

    public UserId UserId { get; }

    public DateOnly Date { get; private set; }

    public Amount Amount { get; private set; }

    public SubmissionMethod? SubmissionMethod { get; private set; }

    public ReceiptId? ReceiptId { get; private set; }

    public TraveringInformation? TraveringInformation { get; }

    public SundryInformation? SundryInformation { get; }

    public void Update(DateOnly date, Amount amount, SubmissionMethod? submissionMethod, ReceiptId? receiptId)
    {
        Date = date;
        Amount = amount;
        SubmissionMethod = submissionMethod;
        ReceiptId = submissionMethod == SubmissionMethod.Electronics ? receiptId : null;
    }
}