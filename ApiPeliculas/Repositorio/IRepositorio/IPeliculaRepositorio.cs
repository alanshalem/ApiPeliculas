// En la interfaz definimos los métodos que después crearemos su funcionalidad
using ApiPeliculas.Modelos;

namespace ApiPeliculas.Repositorio.IRepositorio
{
    public interface IPeliculaRepositorio
    {
        #region Consultas Generales
        ICollection<Pelicula> GetPeliculas();
        Pelicula GetPelicula(int peliculaId);
        ICollection<Pelicula> BuscarPelicula(string nombre);
        ICollection<Pelicula> GetPeliculasEnCategoria(int categoriaId);
        #endregion

        #region Métodos de Validación
        bool ExistePelicula(string nombre);
        bool ExistePelicula(int id);
        #endregion

        #region Operaciones CRUD
        bool CrearPelicula(Pelicula pelicula);
        bool ActualizarPelicula(Pelicula pelicula);
        bool BorrarPelicula(Pelicula pelicula);
        #endregion

        #region Guardar Cambios
        bool Guardar();
        #endregion
    }
}
