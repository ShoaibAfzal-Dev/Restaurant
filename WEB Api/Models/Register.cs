using System.ComponentModel.DataAnnotations;

namespace WEB_Api.Models
{
    public class Register
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; }=null!;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password",ErrorMessage ="Password and Confirm Password does not match")]
        public string ConfirmPassword { get; set;} = null!;
    }
}
