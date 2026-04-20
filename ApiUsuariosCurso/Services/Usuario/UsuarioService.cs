using ApiUsuariosCurso.Data;
using ApiUsuariosCurso.DTO.Login;
using ApiUsuariosCurso.DTO.Usuario;
using ApiUsuariosCurso.Models;
using ApiUsuariosCurso.Services.Senha;
using Microsoft.EntityFrameworkCore;

namespace ApiUsuariosCurso.Services.Usuario
{
    public class UsuarioService : IUsuarioInterface
    {
        private readonly AppDbContext _context;
        private readonly ISenhaInterface _senhaInterface;
        public UsuarioService(AppDbContext context, ISenhaInterface senhaInterface)
        {
            _context = context;
            _senhaInterface = senhaInterface;
        }

        public async Task<ResponseModel<UsuarioModel>> EditarUsuario(UsuarioEdicaoDTO usuarioEdicaoDTO)
        {
           ResponseModel<UsuarioModel> response = new ResponseModel<UsuarioModel>();
            try
            {
                var usuario = await _context.Usuarios.FindAsync(usuarioEdicaoDTO.ID);
                if (usuario != null)
                {
                    usuario.Usuario = usuarioEdicaoDTO.Usuario;
                    usuario.Nome = usuarioEdicaoDTO.Nome;
                    usuario.Sobrenome = usuarioEdicaoDTO.Sobrenome;
                    usuario.Email = usuarioEdicaoDTO.Email;
                    usuario.DataAlteracao = DateTime.Now;
                    _context.Usuarios.Update(usuario);
                    await _context.SaveChangesAsync();
                    response.Dados = usuario;
                    response.Mensagem = "Usuário editado com sucesso.";
                }
                else
                {
                    response.Mensagem = "Usuário não encontrado.";
                    response.Status = false;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<UsuarioModel>> ExcluirUsuario(int id)
        {
            ResponseModel<UsuarioModel> response = new ResponseModel<UsuarioModel>();
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario != null)
                {
                    _context.Usuarios.Remove(usuario);
                    await _context.SaveChangesAsync();
                    response.Dados = usuario;
                    response.Mensagem = "Usuário excluído com sucesso.";
                }
                else
                {
                    response.Mensagem = "Usuário não encontrado.";
                    response.Status = false;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }         
        }

        public async Task<ResponseModel<List<UsuarioModel>>> ListarUsuarios()
        {
            ResponseModel<List<UsuarioModel>> response = new ResponseModel<List<UsuarioModel>>();

            try
            {
                var ususarios = await _context.Usuarios.ToListAsync();
                response.Dados = ususarios;
                response.Mensagem = "Lista de usuários obtida com sucesso.";                

                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<UsuarioModel>> LoginUsuario(UsuarioLoginDTO usuarioLoginDTO)
        { 
            ResponseModel<UsuarioModel> response = new ResponseModel<UsuarioModel>();
            try
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == usuarioLoginDTO.Email);
                if (usuario == null)
                {
                    response.Mensagem = "Usuário não encontrado.";
                    response.Status = false;
                    return response;
                }
               
                if (!_senhaInterface.VerificarSenhaHash(usuarioLoginDTO.Senha, usuario.SenhaHash, usuario.SenhaSalt))
                {
                    response.Mensagem = "Credenciais incorretas.";
                    response.Status = false;
                    return response;
                }
                               
                usuario.Token = _senhaInterface.CriarToken(usuario);
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();

                response.Dados = usuario;
                response.Mensagem = "Login realizado com sucesso.";

                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }

        }

        public async Task<ResponseModel<UsuarioModel>> ObterUsuarioPorId(int id)
        {
            ResponseModel<UsuarioModel> response = new ResponseModel<UsuarioModel>();
            try
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);               
                if (usuario != null)
                {
                    response.Dados = usuario;
                    response.Mensagem = "Usuário encontrado.";
                }
                else
                {
                    response.Mensagem = "Usuário não encontrado.";
                    response.Status = false;
                }
                return response;

            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<UsuarioModel>> RegistrarUsuario(UsuarioCriacaoDTO usuarioCriacaoDTO)
        {
            ResponseModel<UsuarioModel> response = new ResponseModel<UsuarioModel>();

            try
            {
                if(!VeridicaSeExisteEmailUsuarioRepetidos(usuarioCriacaoDTO))
                {
                    response.Mensagem = "Email ou Usuario já existe.";
                    response.Status = false;
                    return response;
                }
                _senhaInterface.CriarSenhaHash(usuarioCriacaoDTO.Senha, out byte[] senhaHash, out byte[] senhaSalt);//Gerar o hash e salt da senha utilizando a interface de senha
                                                                                                                    //out byte[] senhaHash, out byte[] senhaSalt são preenchidos
                                                                                                                    //por conta do método CriarSenhaHash, ou seja,
                                                                                                                    //o método irá gerar o hash e salt da senha e
                                                                                                                    //preencher as variáveis senhaHash e senhaSalt
                                                                                                                    //com os valores gerados e por conta do out posso acessar os valores.
                UsuarioModel usuario = new UsuarioModel
                {
                    Usuario = usuarioCriacaoDTO.Usuario,
                    Nome = usuarioCriacaoDTO.Nome,
                    Sobrenome = usuarioCriacaoDTO.Sobrenome,
                    Email = usuarioCriacaoDTO.Email,
                    SenhaHash = senhaHash,
                    SenhaSalt = senhaSalt
                };
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
                response.Mensagem = "Usuário registrado com sucesso.";
                response.Dados = usuario;
                return response;
            }
            catch (Exception ex) 
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        private bool VeridicaSeExisteEmailUsuarioRepetidos(UsuarioCriacaoDTO usuarioCriacaoDTO)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == usuarioCriacaoDTO.Email ||
                                                                u.Usuario == usuarioCriacaoDTO.Usuario);
            if(usuario != null)
            {
                return false;
            }
            return true;
        }

    }
}
