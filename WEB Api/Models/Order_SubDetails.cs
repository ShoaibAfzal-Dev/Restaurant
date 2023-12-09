using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_Api.Models
{
    public class Order_SubDetails
    {
        [Key]
        public int Id { get; set; }
        public string details { get; set; } = null!;
        public int price { get; set; }
        [ForeignKey("Order_Details")]
        public int Order_DetailsId { get; set; }

    }
}
