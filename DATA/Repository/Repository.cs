using System;
using System.Linq;

using SIRS.Domain.Interfaces;
using SIRS.Data.Context;

using Microsoft.EntityFrameworkCore;

namespace SIRS.Data.Repository;

public abstract class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    protected readonly ApplicationDbContext _db;
    protected readonly DbSet<TEntity> _dbSet;


    public Repository(ApplicationDbContext context)
    {
        _db = context;
        _dbSet = _db.Set<TEntity>();
    }

    public virtual void Add(TEntity obj)
    {
        _dbSet.Add(obj);
    }

    public virtual TEntity GetById(Guid id)
    {
        return _dbSet.Find(id);
    }

    public virtual IQueryable<TEntity> GetAll()
    {
        return _dbSet;
    }

    public virtual IQueryable<TEntity> GetAll(ISpecification<TEntity> spec)
    {
        return ApplySpecification(spec);
    }

    public virtual IQueryable<TEntity> GetAllSoftDeleted()
    {
        return _dbSet.IgnoreQueryFilters()
            .Where(e => EF.Property<bool>(e, "IsDeleted") == true);
    }

    public virtual void Update(TEntity obj)
    {
        _dbSet.Update(obj);
    }

    public virtual void Remove(Guid id)
    {
        _dbSet.Remove(_dbSet.Find(id));
    }

    public int SaveChanges()
    {
        return _db.SaveChanges();
    }

    public void Dispose()
    {
        _db.Dispose();
        GC.SuppressFinalize(this);
    }

    private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
    {
        return SpecificationEvaluator<TEntity>.GetQuery(_dbSet.AsQueryable(), spec);
    }
}
