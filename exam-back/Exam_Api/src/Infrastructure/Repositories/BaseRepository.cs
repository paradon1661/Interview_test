namespace Exam.Infrastructure.Repositories;

public interface IBaseRepository<T> where T : class
{
    void Add(T entity);
    void Remove(T entity);
    /// <summary>One transaction.</summary>
    Task SaveChangesAsync(CancellationToken ct);
}

/// <summary>Common plumbing for module repositories. Queries stay in the concrete repository, written for their use case.</summary>
public abstract class BaseRepository<T>(ApplicationDbContext db) : IBaseRepository<T> where T : class
{
    protected ApplicationDbContext Db { get; } = db;

    public virtual void Add(T entity) => Db.Set<T>().Add(entity);

    public virtual void Remove(T entity) => Db.Set<T>().Remove(entity);

    public Task SaveChangesAsync(CancellationToken ct) => Db.SaveChangesAsync(ct);
}
