namespace CryptoProj.Storage.Repositories;

public abstract class BaseRepository<TEntity> where TEntity : class
{
    protected readonly CryptoContext Context;

    protected BaseRepository(CryptoContext context)
    {
        Context = context;
    }
    
    public ValueTask<TEntity?> Get(int id)
    {
        return Context.Set<TEntity>().FindAsync(id);
    }
    
    public async Task<TEntity> Add(TEntity entity)
    {
        Context.Set<TEntity>().Add(entity);
        await Context.SaveChangesAsync();
        return entity;
    }
}