using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Quic;

namespace WEB_Api.Models
{
    public class Order_Details
    {
        [Key]
        public int Id { get; set; }
        public bool OrderStatus { get; set; } = false;
        public string? UserId { get; set; }
        public int quantity { get; set; }
        public string item { get; set; } = null!;
        public int Price { get; set; }
        public string Instructions { get; set; }
        public bool Status { get; set; }
        [ForeignKey("User_details")]
        public int? User_detailsId { get; set; }
    }
}
