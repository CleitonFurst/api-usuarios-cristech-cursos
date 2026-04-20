using System.ComponentModel.DataAnnotations;

namespace ApiUsuariosCurso.DTO.Usuario
{
    public class UsuarioEdicaoDTO
    {
        [Required(ErrorMessage = "O campo 'ID' é obrigatório.")]
        public int ID { get; set; }
        [Required(ErrorMessage = "O campo 'Usuario' é obrigatório.")]
        public string Usuario { get; set; } = string.Empty;
        [Required(ErrorMessage = "O campo 'Senha' é obrigatório.")]
        public string Nome { get; set; } = string.Empty;
        [Required(ErrorMessage = "O campo 'Sobrenome' é obrigatório.")]
        public string Sobrenome { get; set; } = string.Empty;
        [Required(ErrorMessage = "O campo 'Email' é obrigatório.")]
        public string Email { get; set; } = string.Empty;

        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime DataAlteracao { get; set; } = DateTime.Now;

    }
}
