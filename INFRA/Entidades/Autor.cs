namespace INFRA.Entidades;

public class Autor
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Biografia { get; set; }
    public ICollection<LivroAutor> LivrosAutores { get; set; }
}