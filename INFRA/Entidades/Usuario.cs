namespace INFRA.Entidades;

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public byte[] SenhaHash { get; set; }
    public byte[] SenhaSalt { get; set; }
    public string Perfil { get; set; }
    public ICollection<Avaliacao> Avaliacoes { get; set; }
}