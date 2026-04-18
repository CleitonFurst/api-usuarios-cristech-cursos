using ApiUsuariosCurso.Data;
using ApiUsuariosCurso.DTO.Usuario;
using ApiUsuariosCurso.Models;
using ApiUsuariosCurso.Services.Senha;

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
