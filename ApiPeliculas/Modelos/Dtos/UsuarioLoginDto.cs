using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos.Dtos
{
    public class UsuarioLoginDto
    {

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [MaxLength(60, ErrorMessage = "El número máximo de caracteres es 60")]
        [MinLength(5, ErrorMessage = "El nombre de usuario debe tener al menos 5 caracteres")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MaxLength(60, ErrorMessage = "El número máximo de caracteres es 60")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Password { get; set; }
    }
}
