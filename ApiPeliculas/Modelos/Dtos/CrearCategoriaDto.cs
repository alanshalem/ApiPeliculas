using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos.Dtos
{
    public class CrearCategoriaDto
    {
        [Required(ErrorMessage = "El nombre de la categoria es obligatorio")]
        [MaxLength(60, ErrorMessage = "El numero maximo de caracteres es 60")]
        public string Nombre { get; set; }
    }
}
