// En la interfaz definimos los metodos que despues crearemos su funcionalidad
using ApiPeliculas.Modelos;

namespace ApiPeliculas.Repositorio.IRepositorio
{
    public interface ICategoriaRepositorio
    {
        // Metodo para obtener todas las categorias
        ICollection<Categoria> GetCategorias();
        // Metodo para obtener una categoria
        Categoria GetCategoria(int CategoriaId);
        // Metodo para verificar si una categoria existe
        bool ExisteCategoria(string nombre);
        // Metodo para verificar si una categoria existe
        bool ExisteCategoria(int id);
        // Metodo para crear una categoria
        bool CrearCategoria(Categoria categoria);
        // Metodo para actualizar una categoria
        bool ActualizarCategoria(Categoria categoria);
        // Metodo para borrar una categoria
        bool BorrarCategoria(Categoria categoria);
        // Metodo para guardar los cambios
        bool Guardar();
    }
}
