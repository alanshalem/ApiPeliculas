using ApiPeliculas.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Repositorio
{
    public class PeliculaRepositorio : IPeliculaRepositorio
    {
        private readonly ApplicationDbContext _context;
        public PeliculaRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool ActualizarPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;
            _context.Pelicula.Update(pelicula);
            return Guardar();
        }

        public bool BorrarPelicula(Pelicula pelicula)
        {
            _context.Pelicula.Remove(pelicula);
            return Guardar();
        }

        public bool CrearPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;
            _context.Pelicula.Add(pelicula);
            return Guardar();
        }

        public ICollection<Pelicula> BuscarPelicula(string nombre)
        {
            IQueryable<Pelicula> query = _context.Pelicula;

            if (!string.IsNullOrEmpty(nombre)) {
                query = query.Where(e => e.Nombre.Contains(nombre) || e.Descripcion.Contains(nombre));

            }
            return query.ToList();
        }

        public bool ExistePelicula(string nombre)
        {
            bool existe = false;
            existe = _context.Pelicula.Any(Pelicula => Pelicula.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return existe;
        }

        public bool ExistePelicula(int id)
        {
            return _context.Pelicula.Any(Pelicula => Pelicula.Id == id);
        }

        public Pelicula GetPelicula(int peliculaId)
        {
            return _context.Pelicula.FirstOrDefault(Pelicula => Pelicula.Id == peliculaId);
        }

        public ICollection<Pelicula> GetPeliculas()
        {
            return _context.Pelicula.OrderBy(Pelicula => Pelicula.Id).ToList(); // Ordena las Peliculas por Id
        }

        public ICollection<Pelicula> GetPeliculasEnCategoria(int categoriaId)
        {
            return _context.Pelicula
                           .Where(p => p.categoriaId == categoriaId)  // Filtra las películas por categoría
                           .OrderBy(p => p.Id)                       // Ordena las películas por Id
                           .ToList();                               // Convierte el resultado en una lista
        }

        public ICollection<Pelicula> BuscarPeliculasPorNombre(string nombre)
        {
            return _context.Pelicula
                           .Include(p => p.Categoria)                                                    // Incluye la categoría asociada
                           .Where(p => p.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase))  // Filtra las películas por nombre (insensible a mayúsculas/minúsculas)
                           .OrderBy(p => p.Id)                                                          // Ordena las películas por Id
                           .ToList();                                                                  // Convierte el resultado en una lista
        }

        public bool Guardar()
        {
            try
            {
                return _context.SaveChanges() > 0;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error al guardar cambios: {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
