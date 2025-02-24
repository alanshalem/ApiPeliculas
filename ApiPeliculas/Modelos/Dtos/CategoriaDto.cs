using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos.Dtos
{
    public class CategoriaDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre de la categoria es obligatorio")]
        [MaxLength(60, ErrorMessage = "El numero maximo de caracteres es 60")]
        public string Nombre { get; set; }
    }
}
