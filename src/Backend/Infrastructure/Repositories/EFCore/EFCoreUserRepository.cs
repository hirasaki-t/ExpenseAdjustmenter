using Domain.Models.Users;
using Domain.Repositories;
using Infrastructure.Repositories.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Infrastructure.Repositories.EFCore;

public class EFCoreUserRepository : EFCoreRepositoryBase, IUserRepository
{
    public EFCoreUserRepository(ContextBase context) : base(context)
    {
    }

    public async Task<User?> GetAsync(UserId id) =>
        await context.Users.SingleOrDefaultAsync(x => x.Id == id);

    public async Task<User?> GetAsync(Mail mail) =>
        await context.Users.SingleOrDefaultAsync(x => x.Mail == mail);

    public async Task<IDictionary<UserId, User>> GetDictionaryAsync() =>
        await context.Users.Where(x => x.IsActive).ToDictionaryAsync(x => x.Id, x => x);

    public async Task AddAsync(User user) => await context.AddAsync(user);

    public async Task UpdateAsync(User user)
    {
        if (!await context.Users.AnyAsync(x => x.Id == user.Id)) throw new ArgumentException("更新対象のオブジェクトが存在しません。");
        context.Users.Update(user);
    }
}