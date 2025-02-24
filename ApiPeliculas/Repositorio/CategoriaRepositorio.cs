using ApiPeliculas.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.IRepositorio;

namespace ApiPeliculas.Repositorio
{
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly ApplicationDbContext _context;
        public CategoriaRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool ActualizarCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;
            _context.Categoria.Update(categoria);
            return Guardar();
        }

        public bool BorrarCategoria(Categoria categoria)
        {
            _context.Categoria.Remove(categoria);
            return Guardar();
        }

        public bool CrearCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;
            _context.Categoria.Add(categoria);
            return Guardar();
        }

        public bool ExisteCategoria(string nombre)
        {
            bool existe = false;
            existe = _context.Categoria.Any(categoria => categoria.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return existe;
        }

        public bool ExisteCategoria(int id)
        {
            return _context.Categoria.Any(categoria => categoria.Id == id);
        }

        public Categoria GetCategoria(int CategoriaId)
        {
            return _context.Categoria.FirstOrDefault(categoria => categoria.Id == CategoriaId);
        }

        public ICollection<Categoria> GetCategorias()
        {
            return _context.Categoria.OrderBy(categoria => categoria.Id).ToList(); // Ordena las categorias por Id
        }

        public bool Guardar()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
