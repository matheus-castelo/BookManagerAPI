namespace INFRA.Repositorios.Interfaces;

public interface IRepositoryBase<T> where T : class
{
    Task<T> ObterPorIdAsync(int id);
    Task<IEnumerable<T>> ObterTodosAsync();
    Task AdicionarAsync(T entity);
    void Atualizar(T entity);
    void Remover(T entity);
    IQueryable<T> Query(); 
}