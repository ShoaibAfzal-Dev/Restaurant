using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_Api.Models
{
    public class Dproducts
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int price { get; set; }
        public int Quantity { get; set; }
        public string? CategoryName { get; set; }=null!;
    }
}
