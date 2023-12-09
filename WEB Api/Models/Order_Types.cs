using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_Api.Models
{
    public class Order_Types
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [ForeignKey("Products")]
        public int ProductId { get; set; }
        [NotMapped]
        public Products Products { get; set; }=null!;
    }
}