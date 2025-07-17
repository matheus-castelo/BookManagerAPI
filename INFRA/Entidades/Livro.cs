namespace INFRA.Entidades;

public class Livro
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public string ISBN { get; set; }
    public DateTime DataPublicacao { get; set; }
    public decimal MediaAvaliacao { get; set; }
    public ICollection<LivroAutor> LivrosAutores { get; set; }
    public ICollection<LivroGenero> LivrosGeneros { get; set; }
    public ICollection<Avaliacao> Avaliacoes { get; set; }
}