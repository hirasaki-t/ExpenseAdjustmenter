using Domain.Models.Users;

namespace Domain.Services;

public interface IUserNameSearcher
{
    Task<UserName?> SearchAsync(Mail mail);
}