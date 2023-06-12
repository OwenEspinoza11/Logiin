using System.ComponentModel.DataAnnotations;

namespace Login.Models.Dto
{
    public class CrearUsuarioDto
    {
        [Required]
        [MaxLength(40)]
        public string? UserName { get; set; }
        [MaxLength(40)]
        public string? Password { get; set; }

    }
}
