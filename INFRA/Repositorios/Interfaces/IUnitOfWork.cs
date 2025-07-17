using INFRA.Repositorios.Interfaces;

namespace INFRA.Repositorios
{
    public interface IUnitOfWork
    {
        IUsuarioRepository UsuarioRepository { get; }
        ILivroRepository LivroRepository { get; }
        IAutorRepository AutorRepository { get; }
        IGeneroRepository GeneroRepository { get; }
        IAvaliacaoRepository AvaliacaoRepository { get; }
        Task CommitAsync();
    }
}