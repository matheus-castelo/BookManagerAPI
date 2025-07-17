using INFRA.Data;
using INFRA.Entidades;
using INFRA.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace INFRA.Repositorios;

public class AutorRepository : RepositoryBase<Autor>, IAutorRepository
{
    public AutorRepository(AppDbContext context) : base(context) { }
}

public class GeneroRepository : RepositoryBase<Genero>, IGeneroRepository
{
    public GeneroRepository(AppDbContext context) : base(context) { }
}


