using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Models
{
    public class Main_Order
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int ProductId { get; set; }
    }
}
