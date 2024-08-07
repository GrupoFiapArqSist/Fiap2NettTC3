﻿using Microsoft.EntityFrameworkCore;
using TicketNow.Domain.Interfaces.Entities;
using TicketNow.Domain.Interfaces.Repositories;

namespace TicketNow.Infra.Data.Repositories;

public abstract class BaseRepository<TObject, G, TContext> : IBaseRepository<TObject, G>
   where TObject : class, IEntity<G>
   where TContext : DbContext
{
    protected TContext _dataContext;

    public BaseRepository(TContext context)
    {
        _dataContext = context;
    }

    public void Insert(TObject obj)
    {
        _dataContext.Set<TObject>().Add(obj);
        _dataContext.SaveChanges();
    }

    public void Update(TObject obj)
    {
        _dataContext.Entry(obj).State = EntityState.Modified;
        _dataContext.SaveChanges();
    }

    public void Delete(G id)
    {
        _dataContext.Set<TObject>().Remove(Select(id));
        _dataContext.SaveChanges();
    }

    public IList<TObject> Select() =>
        _dataContext.Set<TObject>().ToList();

    public async Task<IList<TObject>> SelectAsync() =>
        await _dataContext.Set<TObject>().ToListAsync();

    public TObject Select(G id) =>
        _dataContext.Set<TObject>().Find(id);

    public async Task<int> SaveChangesAsync()
    {
        return await _dataContext.SaveChangesAsync();
    }
}
