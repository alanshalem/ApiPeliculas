using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Modelos
{
    public class Pelicula
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string RutaImagen { get; set; }
        public string Descripcion { get; set; }
        public int Duracion { get; set; }
        public enum TipoClasificacion { Siete, Trece, Dieciseis, Dieciocho }
        public TipoClasificacion Clasificacion { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Clave foránea que hace referencia a la entidad 'Categoria'
        [ForeignKey("categoriaId")] // Indica que 'categoriaId' es la clave foránea
        public int categoriaId { get; set; } // Guarda el ID de la categoría asociada a la película

        // Propiedad de navegación que permite acceder a la información completa de la categoría
        public Categoria Categoria { get; set; }
        // 'Categoria' es una instancia de la clase Categoria que representa la relación entre las tablas.
        // Esto permite acceder a los datos completos de la categoría desde una película.
    }
}
