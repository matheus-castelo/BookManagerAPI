using INFRA.Data;
using INFRA.Entidades;
using INFRA.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace INFRA.Repositorios;

public class LivroRepository : RepositoryBase<Livro>, ILivroRepository
{
    public LivroRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Livro>> ObterLivrosPorGeneroAsync(int generoId)
    {
        return await _context.Livros
            .Where(l => l.LivrosGeneros.Any(lg => lg.GeneroId == generoId))
            .ToListAsync();
    }

    public async Task<IEnumerable<Livro>> ObterLivrosPorAutorAsync(int autorId)
    {
        return await _context.Livros
            .Where(l => l.LivrosAutores.Any(la => la.AutorId == autorId))
            .ToListAsync();
    }

    public async Task<IEnumerable<Livro>> ObterTopLivrosAvaliadosAsync(int quantidade)
    {
        return await _context.Livros
            .Select(l => new
            {
                Livro = l,
                Media = (double)l.MediaAvaliacao
            })
            .OrderByDescending(x => x.Media)
            .Take(quantidade)
            .Select(x => x.Livro)
            .ToListAsync();
    }

}