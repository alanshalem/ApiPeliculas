using ApiPeliculas.Modelos;

namespace ApiPeliculas.Repositorio.IRepositorio
{
    public interface ICategoriaRepositorio
    {
        #region Consultas Generales
        ICollection<Categoria> GetCategorias();
        Categoria GetCategoria(int CategoriaId);
        #endregion

        #region Metodos de Validacion
        bool ExisteCategoria(string nombre);
        bool ExisteCategoria(int id);
        #endregion

        #region Operaciones CRUD
        bool CrearCategoria(Categoria categoria);
        bool ActualizarCategoria(Categoria categoria);
        bool BorrarCategoria(Categoria categoria);
        #endregion

        #region Guardado
        bool Guardar();
        #endregion
    }
}
