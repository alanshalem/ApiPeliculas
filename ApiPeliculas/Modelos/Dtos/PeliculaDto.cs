using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Modelos.Dtos
{
    public class PeliculaDto
    {
        [Required(ErrorMessage = "El nombre de la pelicula es obligatorio")]
        [MaxLength(60, ErrorMessage = "El numero máximo de caracteres es 60")]
        public string Nombre { get; set; }
        public string RutaImagen { get; set; }
        [Required(ErrorMessage = "La descripción de la pelicula es obligatoria")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "La duración de la pelicula es obligatorio")]
        public int Duracion { get; set; }
        public enum TipoClasificacion { Siete, Trece, Dieciseis, Dieciocho }
        public TipoClasificacion Clasificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int categoriaId { get; set; }
    }
}
