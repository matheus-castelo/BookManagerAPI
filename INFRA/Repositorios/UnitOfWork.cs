using INFRA.Data;
using INFRA.Repositorios.Interfaces;

namespace INFRA.Repositorios
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            UsuarioRepository = new UsuarioRepository(context);
            LivroRepository = new LivroRepository(context);
            AutorRepository = new AutorRepository(context);
            GeneroRepository = new GeneroRepository(context);
            AvaliacaoRepository = new AvaliacaoRepository(context);
        }

        public IUsuarioRepository UsuarioRepository { get; }
        public ILivroRepository LivroRepository { get; }
        public IAutorRepository AutorRepository { get; }
        public IGeneroRepository GeneroRepository { get; }
        public IAvaliacaoRepository AvaliacaoRepository { get; }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}