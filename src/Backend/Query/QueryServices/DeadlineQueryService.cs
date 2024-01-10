namespace Query.QueryServices;

public class DeadlineQueryService
{
    private readonly DateOnly date;

    public DeadlineQueryService(string? value)
    {
        var today = DateTime.Now;
        if (!int.TryParse(value, out var day)) throw new InvalidOperationException("締日の設定値が異常な値になっています。");
        date = DateTime.Now.Day <= day ? new DateOnly(today.AddMonths(-1).Year, today.AddMonths(-1).Month, day) : new DateOnly(today.Year, today.Month, day);
    }

    public DateOnly GetDeadline() => date;
}