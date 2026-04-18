namespace ApiUsuariosCurso.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        public required string Usuario { get; set; }
        public required string Nome { get; set; }
        public required string Sobrenome { get; set; }
        public required string Email { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime DataAlteracao { get; set; } = DateTime.Now;
        public byte[] SenhaHash { get; set; } = [];
        public byte[] SenhaSalt { get; set; } = [];
    }
}
