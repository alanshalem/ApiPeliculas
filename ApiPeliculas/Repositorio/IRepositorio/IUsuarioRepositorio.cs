using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;

namespace ApiPeliculas.Repositorio.IRepositorio
{
    public interface IUsuarioRepositorio
    {
        #region Consultas Generales
        ICollection<Usuario> GetUsuarios();
        Usuario GetUsuario(int usuarioId);
        #endregion

        #region Metodos de Validacion
        bool IsUniqueUser(string usuario);
        #endregion
        #region Login & Register
        Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto);
        Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto);
        #endregion
    }
}
