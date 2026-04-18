using ApiUsuariosCurso.DTO.Usuario;
using ApiUsuariosCurso.Models;

namespace ApiUsuariosCurso.Services.Usuario
{
    public interface IUsuarioInterface 
    {
        Task<ResponseModel<UsuarioModel>> RegistrarUsuario(UsuarioCriacaoDTO usuarioCriacaoDTO);

    }
}
