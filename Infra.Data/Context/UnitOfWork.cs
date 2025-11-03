using System.Diagnostics.CodeAnalysis;

namespace Infra.Data.Context;
[ExcludeFromCodeCoverage]
public class UnitOfWork(CustomDbContext customDbContext) : IUnitOfWork, IDisposable
{
    public CustomDbContext CustomDbContext { get; } = customDbContext;

    public CustomDbContext GetContext() => CustomDbContext;
    public void SaveChanges()
    {
        CustomDbContext.SaveChanges();
    }
    public async Task SaveChangesAsync()
    {
        await CustomDbContext.SaveChangesAsync();
    }

    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!this._disposed && disposing)
        {
            CustomDbContext.Dispose();
        }
        this._disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

