namespace Domain.Services;

public interface ILoginUserGetter
{
    Task<string> GetMailAsync();

    Task<string> GetNameAsync();
}