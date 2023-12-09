using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class Login
    {
        [Required]
        public string userName { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
