using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_Api.Models
{
    public class Products
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int price { get; set; }
        public int Quantity { get; set; }
        [ForeignKey(nameof(Category))]  
        public int Categoryid { get; set; }
        public Category Category { get; set; }=null!;
    }
}
