namespace INFRA.Entidades;

public class Genero
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public ICollection<LivroGenero> LivrosGeneros { get; set; }
}