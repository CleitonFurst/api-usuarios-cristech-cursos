using ApiUsuariosCurso.DTO.Login;
using ApiUsuariosCurso.DTO.Usuario;
using ApiUsuariosCurso.Models;

namespace ApiUsuariosCurso.Services.Usuario
{
    public interface IUsuarioInterface 
    {
        Task<ResponseModel<UsuarioModel>> RegistrarUsuario(UsuarioCriacaoDTO usuarioCriacaoDTO);
        Task<ResponseModel<List<UsuarioModel>>> ListarUsuarios();
        Task<ResponseModel<UsuarioModel>> ObterUsuarioPorId(int id);
        Task<ResponseModel<UsuarioModel>> EditarUsuario(UsuarioEdicaoDTO usuarioEdicaoDTO);
        Task<ResponseModel<UsuarioModel>> ExcluirUsuario(int id);
        Task<ResponseModel<UsuarioModel>> LoginUsuario(UsuarioLoginDTO usuarioLoginDTO);


    }
}
