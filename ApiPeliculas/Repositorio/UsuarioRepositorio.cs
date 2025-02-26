using ApiPeliculas.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XAct.Library.Constants;
using XSystem.Security.Cryptography;

namespace ApiPeliculas.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ApplicationDbContext _context;
        private string claveSecreta;
        public UsuarioRepositorio(ApplicationDbContext context, IConfiguration config)
        {
            claveSecreta = config.GetValue<string>("ApiSettings:Secreta");
            _context = context;
        }

        #region Consultas Generales

        public ICollection<Usuario> GetUsuarios()
        {
            return _context.Usuario.OrderBy(u => u.NombreUsuario).ToList();
        }

        public Usuario GetUsuario(int usuarioId)
        {
            return _context.Usuario.FirstOrDefault(u => u.Id == usuarioId);
        }

        #endregion

        #region Metodos de Validacion

        public bool IsUniqueUser(string usuario)
        {
            var usuariobd = _context.Usuario.FirstOrDefault(u => u.NombreUsuario == usuario);
            if (usuariobd == null)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region Login & Register
        public async Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto)
        {
            var passwordEncriptado = obtenermd5(usuarioRegistroDto.Password);

            if (IsUniqueUser(usuarioRegistroDto.NombreUsuario))
            {
                var usuario = new Usuario()
                {
                    NombreUsuario = usuarioRegistroDto.NombreUsuario,
                    Password = passwordEncriptado, // Asegúrate de cifrar la contraseña
                    Nombre = usuarioRegistroDto.Nombre,
                    Role = usuarioRegistroDto.Role
                };

                // Guardar en la base de datos
                _context.Usuario.Add(usuario);
                await _context.SaveChangesAsync();
                usuario.Password = passwordEncriptado;
                return usuario;
            }

            return null; // El nombre de usuario no es único
        }

        public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto)
        {
            // Encripta la contraseña ingresada por el usuario para compararla con la almacenada en la base de datos
            var passwordEncriptado = obtenermd5(usuarioLoginDto.Password);

            // Busca un usuario en la base de datos con el mismo nombre de usuario y contraseña encriptada
            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(
                    u => u.NombreUsuario.ToLower() == usuarioLoginDto.NombreUsuario.ToLower()
                    && u.Password == passwordEncriptado
                );

            // Si no se encuentra el usuario, devuelve una respuesta con un token vacío y usuario nulo
            if (usuario == null)
                return new UsuarioLoginRespuestaDto()
                {
                    Token = "",
                    Usuario = null
                };

            // Crea un manejador de tokens JWT
            var manejadorToken = new JwtSecurityTokenHandler();

            // Convierte la clave secreta en un array de bytes para firmar el token
            var key = Encoding.ASCII.GetBytes(claveSecreta);

            // Configuración del token a generar
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Se define la identidad del usuario dentro del token con sus claims
                // Se define la identidad del usuario dentro del token con sus claims
                // Se define la identidad del usuario dentro del token con sus claims
                // Se define la identidad del usuario dentro del token con sus claims
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.NombreUsuario.ToString()), // Claim con el nombre de usuario
                    new Claim(ClaimTypes.Role, usuario.Role) // Claim con el rol del usuario
                }),




                // El token tendrá una validez de 7 días
                Expires = DateTime.UtcNow.AddDays(7),

                // Se firma el token utilizando la clave secreta y el algoritmo HmacSha256
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            // Se genera el token JWT basado en la configuración definida en el tokenDescriptor
            var token = manejadorToken.CreateToken(tokenDescriptor);

            // Se crea un objeto de respuesta con el token generado y la información del usuario
            UsuarioLoginRespuestaDto usuarioLoginRespuestaDto = new UsuarioLoginRespuestaDto()
            {
                Token = manejadorToken.WriteToken(token), // Se convierte el token a formato string
                Usuario = usuario // Se adjunta la información del usuario autenticado
            };

            // Retorna el objeto de respuesta con el token y los datos del usuario
            return usuarioLoginRespuestaDto;
        }
        #endregion

        #region Helpers
        //Método para encriptar contraseña con MD5 se usa tanto en el Acceso como en el Registro
        public static string obtenermd5(string valor)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
            data = x.ComputeHash(data);
            string resp = "";
            for (int i = 0; i < data.Length; i++)
                resp += data[i].ToString("x2").ToLower();
            return resp;
        }
        #endregion
    }
}
