using INFRA.Entidades;

namespace INFRA.Repositorios.Interfaces;

public interface ILivroRepository : IRepositoryBase<Livro>
{
    Task<IEnumerable<Livro>> ObterLivrosPorGeneroAsync(int generoId);
    Task<IEnumerable<Livro>> ObterLivrosPorAutorAsync(int autorId);
    Task<IEnumerable<Livro>> ObterTopLivrosAvaliadosAsync(int quantidade);
    IQueryable<Livro> Query(); 
}