using INFRA.Data;
using INFRA.Entidades;
using INFRA.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace INFRA.Repositorios;

public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(AppDbContext context) : base(context) { }

    public async Task<Usuario> ObterPorEmailAsync(string email)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
    }

    public new async Task<Usuario> ObterPorIdAsync(int id)
    {
        return await base.ObterPorIdAsync(id);
    }
}