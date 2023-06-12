using System.ComponentModel.DataAnnotations;

namespace Login.Models.Dto
{
    public class UsuarioDto
    {
        public int UserId { get; set; }
        [Required]
        public string? UserName { get; set; }
        public string? Password { get; set; }

    }
}
