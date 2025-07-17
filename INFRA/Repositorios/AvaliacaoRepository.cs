using INFRA.Data;
using INFRA.Entidades;
using INFRA.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace INFRA.Repositorios
{
    public class AvaliacaoRepository 
        : RepositoryBase<Avaliacao>, IAvaliacaoRepository
    {
        public AvaliacaoRepository(AppDbContext context) 
            : base(context)
        { }

        public async Task<IEnumerable<Avaliacao>> ObterAvaliacoesPorLivroAsync(int livroId)
        {
            return await _context.Avaliacoes
                .Where(a => a.LivroId == livroId)
                .ToListAsync();
        }

        public async Task<Avaliacao?> ObterAvaliacaoPorUsuarioELivroAsync(
            int usuarioId, int livroId)
        {
            return await _context.Avaliacoes
                .FirstOrDefaultAsync(a =>
                    a.UsuarioId == usuarioId 
                    && a.LivroId    == livroId);
        }
        
        public async Task<IEnumerable<Avaliacao>> ObterAvaliacoesPorUsuarioAsync(int usuarioId)
            => await _context.Avaliacoes
                .Where(a => a.UsuarioId == usuarioId)
                .ToListAsync();
    }
}