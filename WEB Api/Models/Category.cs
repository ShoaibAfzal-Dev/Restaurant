using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WEB_Api.Models
{
    [Index(nameof(Name),IsUnique =true)]
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}