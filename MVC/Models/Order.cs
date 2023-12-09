using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<Order_Types> Order_SubTypes { get; set; }

    }
}
