namespace INFRA.Entidades;

public class Avaliacao
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int LivroId { get; set; }
    public int Nota { get; set; }
    public string Comentario { get; set; }
    public DateTime DataAvaliacao { get; set; }
    public Usuario Usuario { get; set; }
    public Livro Livro { get; set; }
}