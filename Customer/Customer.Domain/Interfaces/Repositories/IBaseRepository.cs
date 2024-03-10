namespace Customer.Domain.Interfaces.Repositories;

public interface IBaseRepository<T, G>
   where T : class
{
    void Insert(T obj);
    void Update(T obj);
    void Delete(G id);
    IList<T> Select();
    T Select(G id);
    Task<int> SaveChangesAsync();
}
