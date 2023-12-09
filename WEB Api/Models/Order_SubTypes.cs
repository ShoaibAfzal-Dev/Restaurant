using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_Api.Models
{
    public class Order_SubTypes
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int PlusPrice { get; set; }
        public int Count { get; set; }
        [ForeignKey("Order_types")]
        public int Orderid { get; set; }
        [NotMapped]
        public Order_Types Order_Types { get; set; }
    }
}