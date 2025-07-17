

using INFRA.Data;
using INFRA.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace INFRA.Repositorios
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly AppDbContext _context;

        public RepositoryBase(AppDbContext context)
        {
            _context = context;
        }

        public async Task<T> ObterPorIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> ObterTodosAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task AdicionarAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Atualizar(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void Remover(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public IQueryable<T> Query()
        {
            return _context.Set<T>().AsQueryable();

        }

    }
}