using INFRA.Entidades;

namespace INFRA.Repositorios.Interfaces;

public interface IUsuarioRepository : IRepositoryBase<Usuario>
{
    Task<Usuario> ObterPorEmailAsync(string email);
    Task<Usuario> ObterPorIdAsync(int id);
}