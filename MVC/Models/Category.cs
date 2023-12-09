
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class Category
    {  
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public List<products> products { get; set; }
     }
}