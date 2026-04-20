using ApiUsuariosCurso.DTO.Usuario;
using ApiUsuariosCurso.Services.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiUsuariosCurso.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioInterface _usuarioInterface;
        public UsuarioController(IUsuarioInterface usuarioInterface)
        {
            _usuarioInterface = usuarioInterface;
        }
        [HttpGet]

        public async Task<IActionResult> ListarUsuarios()
        {
            var usuarios = await _usuarioInterface.ListarUsuarios();
            return Ok(usuarios);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterUsuarioPorId(int id)
        {
            var usuario = await _usuarioInterface.ObterUsuarioPorId(id);
            if (usuario.Status)
            {
                return Ok(usuario);
            }
            else
            {
                return NotFound(usuario);
            }
        }


        [HttpPut]
        public async Task<IActionResult> EditarUsuario(UsuarioEdicaoDTO usuarioEdicaoDTO)
        {
            var usuario = await _usuarioInterface.EditarUsuario(usuarioEdicaoDTO);
            if (usuario.Status)
            {
                return Ok(usuario);
            }
            else
            {
                return NotFound(usuario);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirUsuario(int id)
        {
            var usuario = await _usuarioInterface.ExcluirUsuario(id);
            if (usuario.Status)
            {
                return Ok(usuario);
            }
            else
            {
                return NotFound(usuario);
            }
        }
    }
}
