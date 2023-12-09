using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Models
{
    public class Order_Types
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public int PlusPrice { get; set; }
        public int Count { get; set; }
        public int Orderid { get; set; }
    }
}
