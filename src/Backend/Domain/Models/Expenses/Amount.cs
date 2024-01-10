namespace Domain.Models.Expenses;

public record Amount
{
    private int value = 0;

    public Amount(int value)
    {
        Value = value;
    }

    public int Value
    {
        get => value;
        private set
        {
            if (value < 0) throw new DomainException("金額は0円以上を入力してください。");
            this.value = value;
        }
    }
}