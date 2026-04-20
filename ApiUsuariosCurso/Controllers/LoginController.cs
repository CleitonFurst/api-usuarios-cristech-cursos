using ApiUsuariosCurso.DTO.Login;
using ApiUsuariosCurso.DTO.Usuario;
using ApiUsuariosCurso.Services.Usuario;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiUsuariosCurso.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUsuarioInterface _usuarioInterface;
        public LoginController(IUsuarioInterface usuarioInterface)
        {
                _usuarioInterface = usuarioInterface;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegistrarUsuario(UsuarioCriacaoDTO usuarioCriacaoDTO)
        {

            var usuario = await _usuarioInterface.RegistrarUsuario(usuarioCriacaoDTO);


            return Ok(usuario);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUsuario(UsuarioLoginDTO usuarioLoginDTO)
        {
            var usuario = await _usuarioInterface.LoginUsuario(usuarioLoginDTO);
            return Ok(usuario);
        }
    }
}
