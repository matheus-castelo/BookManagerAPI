using System.Text;
using INFRA.Data;
using INFRA.Entidades;
using INFRA.Repositorios;
using INFRA.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace TESTES.TestesRepositorios
{
    public class RepositoriosTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly DbContextOptions<AppDbContext> _options;

        public RepositoriosTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            _context = new AppDbContext(_options);
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context.Database.CloseConnection();
            _context.Dispose();
        }

                   private async Task PopularDadosTesteAsync()
            {
                var usuario1 = new Usuario 
                { 
                    Nome = "João", 
                    Email = "joao@teste.com", 
                    SenhaHash = Encoding.UTF8.GetBytes("hash1"), 
                    SenhaSalt = Encoding.UTF8.GetBytes("salt1"), 
                    Perfil = "user" 
                };
                var usuario2 = new Usuario 
                { 
                    Nome = "Admin", 
                    Email = "admin@teste.com", 
                    SenhaHash = Encoding.UTF8.GetBytes("hash2"), 
                    SenhaSalt = Encoding.UTF8.GetBytes("salt2"), 
                    Perfil = "admin" 
                };
                _context.Usuarios.AddRange(usuario1, usuario2);

                var ficcao  = new Genero { Nome = "Ficção" };
                var romance = new Genero { Nome = "Romance" };
                _context.Generos.AddRange(ficcao, romance);

                var autor1 = new Autor { Nome = "Autor A", Biografia = "Bio A" };
                var autor2 = new Autor { Nome = "Autor B", Biografia = "Bio B" };
                _context.Autores.AddRange(autor1, autor2);

                var agora = DateTime.UtcNow;
                var livro1 = new Livro { 
                    Titulo = "Livro 1", 
                    Descricao = "Desc 1", 
                    ISBN = "111", 
                    DataPublicacao = agora, 
                    MediaAvaliacao = 4.5m 
                };
                var livro2 = new Livro { 
                    Titulo = "Livro 2", 
                    Descricao = "Desc 2", 
                    ISBN = "222", 
                    DataPublicacao = agora, 
                    MediaAvaliacao = 3.5m 
                };
                _context.Livros.AddRange(livro1, livro2);

                await _context.SaveChangesAsync();

                _context.LivrosGeneros.AddRange(
                    new LivroGenero { LivroId = livro1.Id, GeneroId = ficcao.Id },
                    new LivroGenero { LivroId = livro2.Id, GeneroId = romance.Id }
                );

                _context.LivrosAutores.AddRange(
                    new LivroAutor { LivroId = livro1.Id, AutorId = autor1.Id },
                    new LivroAutor { LivroId = livro2.Id, AutorId = autor2.Id }
                );

                _context.Avaliacoes.AddRange(
                    new Avaliacao { 
                        UsuarioId     = usuario1.Id, 
                        LivroId       = livro1.Id, 
                        Nota          = 5, 
                        Comentario    = "Ótimo", 
                        DataAvaliacao = agora 
                    },
                    new Avaliacao { 
                        UsuarioId     = usuario1.Id, 
                        LivroId       = livro2.Id, 
                        Nota          = 4, 
                        Comentario    = "Bom", 
                        DataAvaliacao = agora 
                    }
                );

                await _context.SaveChangesAsync();
            }


        [Fact]
        public async Task UsuarioRepository_ObterPorEmailAsync_DeveRetornarUsuario()
        {
            await PopularDadosTesteAsync();
            var repo = new UsuarioRepository(_context);

            var usuario = await repo.ObterPorEmailAsync("joao@teste.com");
            Assert.NotNull(usuario);
            Assert.Equal("João", usuario.Nome);
        }

        [Fact]
        public async Task LivroRepository_ObterLivrosPorGeneroAsync_DeveRetornarLivros()
        {
            await PopularDadosTesteAsync();
            var repo = new LivroRepository(_context);
            var generoId = _context.Generos.First().Id;

            var lista = await repo.ObterLivrosPorGeneroAsync(generoId);
            Assert.Single(lista);
            Assert.Equal("Livro 1", lista.First().Titulo);
        }

        [Fact]
        public async Task LivroRepository_ObterLivrosPorAutorAsync_DeveRetornarLivros()
        {
            await PopularDadosTesteAsync();
            var repo = new LivroRepository(_context);
            var autorId = _context.Autores.First().Id;

            var lista = await repo.ObterLivrosPorAutorAsync(autorId);
            Assert.Single(lista);
            Assert.Equal("Livro 1", lista.First().Titulo);
        }

        [Fact]
        public async Task LivroRepository_ObterTopLivrosAvaliadosAsync_DeveRetornarOrdenado()
        {
            await PopularDadosTesteAsync();
            var repo = new LivroRepository(_context);

            var top = await repo.ObterTopLivrosAvaliadosAsync(2);
            Assert.Equal(2, top.Count());
            Assert.Equal("Livro 1", top.First().Titulo);
            Assert.Equal("Livro 2", top.Last().Titulo);
        }

        [Fact]
        public async Task AutorRepository_ObterTodosAsync_DeveRetornarAutores()
        {
            await PopularDadosTesteAsync();
            var repo = new AutorRepository(_context);

            var autores = await repo.ObterTodosAsync();
            Assert.Equal(2, autores.Count());
        }

        [Fact]
        public async Task GeneroRepository_AdicionarAtualizarRemover_FuncionaCorretamente()
        {
            var repo = new GeneroRepository(_context);
            var genero = new Genero { Nome = "Terror" };

            await repo.AdicionarAsync(genero);
            await _context.SaveChangesAsync();
            Assert.Contains((await _context.Generos.ToListAsync()), g => g.Nome == "Terror");

            genero.Nome = "Terror Novo";
            repo.Atualizar(genero);
            await _context.SaveChangesAsync();
            Assert.Equal("Terror Novo", (await _context.Generos.FindAsync(genero.Id)).Nome);

            repo.Remover(genero);
            await _context.SaveChangesAsync();
            Assert.Null(await _context.Generos.FindAsync(genero.Id));
        }

        [Fact]
        public async Task AvaliacaoRepository_ObterAvaliacoesPorLivroAsync_DeveRetornarAvaliacoes()
        {
            await PopularDadosTesteAsync();
            var repo = new AvaliacaoRepository(_context);
            var livroId = _context.Livros.First().Id;

            var avals = await repo.ObterAvaliacoesPorLivroAsync(livroId);
            Assert.Single(avals);
            Assert.Equal(5, avals.First().Nota);
        }

        [Fact]
        public async Task AvaliacaoRepository_ObterAvaliacoesPorUsuarioAsync_DeveRetornarAvaliacoes()
        {
            await PopularDadosTesteAsync();
            var repo = new AvaliacaoRepository(_context);
            var usuarioId = _context.Usuarios.First().Id;

            var avals = await repo.ObterAvaliacoesPorUsuarioAsync(usuarioId);
            Assert.Equal(2, avals.Count());
        }

        [Fact]
        public async Task RepositoryBase_Query_DevePermitirFiltrar()
        {
            await PopularDadosTesteAsync();
            IRepositoryBase<Livro> repo = new LivroRepository(_context);

            var query = repo.Query().Where(l => l.MediaAvaliacao > 4);
            var lista = await query.ToListAsync();
            Assert.Single(lista);
            Assert.Equal("Livro 1", lista.First().Titulo);
        }

        [Fact]
        public async Task UnitOfWork_CommitAsync_PersisteAlteracoes()
        {
            await PopularDadosTesteAsync();
            var uow = new UnitOfWork(_context);

            var novoGenero = new Genero { Nome = "Suspense" };
            uow.GeneroRepository.AdicionarAsync(novoGenero).Wait();
            await uow.CommitAsync();

            var encontrado = await _context.Generos.FirstOrDefaultAsync(g => g.Nome == "Suspense");
            Assert.NotNull(encontrado);
        }
        
        [Fact]
        public async Task UsuarioRepository_ObterPorIdAsync_DeveRetornarUsuario()
        {
            await PopularDadosTesteAsync();
            var repo = new UsuarioRepository(_context);
            var usuarioId = _context.Usuarios.First().Id;

            var usuario = await repo.ObterPorIdAsync(usuarioId);
            Assert.NotNull(usuario);
            Assert.Equal("João", usuario.Nome);
        }

        [Fact]
        public async Task UsuarioRepository_ObterPorEmailAsync_DeveRetornarNullQuandoNaoExistir()
        {
            await PopularDadosTesteAsync();
            var repo = new UsuarioRepository(_context);

            var usuario = await repo.ObterPorEmailAsync("naoexiste@teste.com");
            Assert.Null(usuario);
        }
        [Fact]
        public async Task AvaliacaoRepository_ObterAvaliacaoPorUsuarioELivroAsync_DeveRetornarNullQuandoNaoExistir()
        {
            await PopularDadosTesteAsync();
            var repo = new AvaliacaoRepository(_context);

            var avaliacao = await repo.ObterAvaliacaoPorUsuarioELivroAsync(999, 999);
            Assert.Null(avaliacao);
        }
        [Fact]
        public async Task LivroRepository_ObterPorIdAsync_DeveRetornarComRelacionamentos()
        {
            await PopularDadosTesteAsync();
            var repo = new LivroRepository(_context);
            var livroId = _context.Livros.First().Id;

            var livro = await repo.ObterPorIdAsync(livroId);
            Assert.NotNull(livro);
            Assert.NotEmpty(livro.LivrosAutores);
            Assert.NotEmpty(livro.LivrosGeneros);
        }
        [Fact]
        public async Task LivroRepository_ObterTopLivrosAvaliadosAsync_DeveRetornarVazioQuandoSemDados()
        {
            var repo = new LivroRepository(_context);
    
            var top = await repo.ObterTopLivrosAvaliadosAsync(5);
            Assert.Empty(top);
        }

        [Fact]
        public async Task AutorRepository_Remover_DeveRemoverRelacionamentos()
        {
            await PopularDadosTesteAsync();
            var repo = new AutorRepository(_context);
            var autor = _context.Autores.First();

            repo.Remover(autor);
            await _context.SaveChangesAsync();

            var livrosComAutor = await _context.LivrosAutores
                .Where(la => la.AutorId == autor.Id)
                .ToListAsync();
            Assert.Empty(livrosComAutor);
        }
        [Fact]
        public async Task UnitOfWork_Rollback_NaoPersisteAlteracoes()
        {
            await PopularDadosTesteAsync();
            var uow = new UnitOfWork(_context);

            var novoGenero = new Genero { Nome = "Terror" };
            await uow.GeneroRepository.AdicionarAsync(novoGenero);

            var encontrado = await _context.Generos.FirstOrDefaultAsync(g => g.Nome == "Terror");
            Assert.Null(encontrado); 
        }
    }
}
