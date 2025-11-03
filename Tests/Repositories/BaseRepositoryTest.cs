using Domain.Entities;
using Infra.Data.Context;
using Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Tests.Mocks;

namespace Tests.Repositories;

public abstract class BaseRepositoryTest<TRepository, TMock, T>
    where TRepository : BaseRepository<T>
    where TMock : BaseMock<T>, new()
    where T : BaseEntity
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly TRepository _repository;
    private readonly TMock _mock = new();

    protected BaseRepositoryTest()
    {
        var options = new DbContextOptionsBuilder<CustomDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new CustomDbContext(options);
        context.Set<T>().AddRange(_mock.GetEnumerable(3));
        context.SaveChanges();
        _unitOfWork.Setup(x => x.GetContext()).Returns(context);
        _repository = (TRepository)Activator.CreateInstance(typeof(TRepository), _unitOfWork.Object)!;
    }

     [Fact]
    public void ShouldGetAll()
    {
        var result = _repository.GetAll();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public void ShouldGet()
    {
        var result = _repository.Get(x => x.Id != Guid.Empty);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task ShouldGetAsync()
    {
        var result = await _repository.GetAsync(x => x.Id != Guid.Empty);

        Assert.NotNull(result);
        var baseEntities = result.ToList();
        Assert.NotEmpty(baseEntities);
        Assert.Equal(3, baseEntities.Count);
    }

    [Fact]
    public async Task ShouldFindByIdAsync()
    {
        var firstEntity = await _repository.GetAll().FirstOrDefaultAsync();
        var entity = await _repository.FindAsync(firstEntity!.Id);

        Assert.NotNull(entity);
        Assert.Equal(firstEntity!.Id, entity.Id);
    }

    [Fact]
    public async Task ShouldGetNullOnFindById()
    {
        var entity = await _repository.FindAsync(Guid.NewGuid());
        Assert.Null(entity);
    }

    [Fact]
    public async Task ShouldFindByPredicateAsync()
    {
        var firstEntity = await _repository.GetAll().FirstOrDefaultAsync();
        var entity = await _repository.FindAsync(x => x.Id == firstEntity!.Id);

        Assert.NotNull(entity);
        Assert.Equal(firstEntity!.Id, entity.Id);
    }

    [Fact]
    public async Task ShouldThrowOnFindByInvalidPredicate()
    {
        var entity = await _repository.FindAsync(x => x.Id == Guid.NewGuid());
        Assert.Null(entity);
    }

    [Fact]
    public async Task ShouldGetNoTrackingAsync()
    {
        var result = await _repository.GetNoTrackingAsync(x => x.Id != Guid.Empty);

        Assert.NotNull(result);
        var baseEntities = result.ToList();
        Assert.NotEmpty(baseEntities);
        Assert.Equal(3, baseEntities.Count);
    }

    [Fact]
    public async Task ShouldCheckAnyAsync()
    {
        var any = await _repository.AnyAsync(x => x.Id != Guid.Empty);

        Assert.True(any);
    }

    [Fact]
    public async Task ShouldInsertAsync()
    {
        var newEntity = _mock.GetEntity();
        var inserted = await _repository.InsertAsync(newEntity);

        Assert.NotNull(inserted);
        Assert.Equal(newEntity.Id, inserted.Id);

        var all = await _repository.GetAll().ToListAsync();
        Assert.Equal(4, all.Count);
    }

    [Fact]
    public async Task ShouldDeleteAsyncById()
    {
        var entity = await _repository.GetAll().FirstOrDefaultAsync();
        await _repository.DeleteAsync(entity!.Id);

        var all = await _repository.GetAll().ToListAsync();
        Assert.Equal(2, all.Count);
        Assert.DoesNotContain(all, e => e.Id == entity.Id);
    }

    [Fact]
    public async Task ShouldDeleteAsyncByEntity()
    {
        var entity = await _repository.GetAll().FirstOrDefaultAsync();
        await _repository.DeleteAsync(entity!);

        var all = await _repository.GetAll().ToListAsync();
        Assert.Equal(2, all.Count);
        Assert.DoesNotContain(all, e => e.Id == entity!.Id);
    }

    [Fact]
    public async Task ShouldGetByCustomer()
    {
        var entity = await _repository.GetAll().FirstOrDefaultAsync();
        var result = await _repository.GetByCreator(entity!.CreatedBy!.Value);
        
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task ShouldGetByCustomerEmptyList()
    {
        var result = await _repository.GetByCreator(Guid.NewGuid());
        
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}