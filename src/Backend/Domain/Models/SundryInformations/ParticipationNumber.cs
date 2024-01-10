namespace Domain.Models.SundryInformations;

public record ParticipationNumber
{
    private int value = 1;

    public ParticipationNumber(int value)
    {
        Value = value;
    }

    public int Value
    {
        get => value;
        private set
        {
            if (value < 1) throw new DomainException("参加人数は1人以上必要です。");
            this.value = value;
        }
    }
}