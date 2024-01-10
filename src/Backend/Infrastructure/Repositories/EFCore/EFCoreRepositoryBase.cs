using Domain.Repositories;
using Infrastructure.Repositories.EFCore.Contexts;

namespace Infrastructure.Repositories.EFCore;

public class EFCoreRepositoryBase : IDisposable, IWritableRepository
{
    protected readonly ContextBase context;

    public EFCoreRepositoryBase(ContextBase context)
    {
        this.context = context;
    }

    public Task CommitAsync()
    {
        return context.SaveChangesAsync();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        context.Dispose();
    }
}