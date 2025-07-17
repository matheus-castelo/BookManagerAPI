using INFRA.Entidades;

namespace INFRA.Repositorios.Interfaces;

public interface IAvaliacaoRepository : IRepositoryBase<Avaliacao>
{
    Task<Avaliacao?> ObterAvaliacaoPorUsuarioELivroAsync(int usuarioId, int livroId);
    Task<IEnumerable<Avaliacao>> ObterAvaliacoesPorLivroAsync(int livroId);
    Task<IEnumerable<Avaliacao>> ObterAvaliacoesPorUsuarioAsync(int usuarioId);
}