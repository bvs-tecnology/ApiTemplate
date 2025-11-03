namespace Infra.Data.Context;
public interface IUnitOfWork
{
    protected CustomDbContext CustomDbContext { get; }
    CustomDbContext GetContext();
    void SaveChanges();
    Task SaveChangesAsync();
}
