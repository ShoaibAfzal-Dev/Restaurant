using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_Api.Models
{
    public class AddToCart
    {
        [Key]
        public int Id { get; set; }
        public int Qty { get; set; }
        public string item { get; set; } = null!;
        public string TotalPrice { get; set; } = null!;
        public string Status { get; set; }
        public int? FinalPriceOfOrder { get; set; }
        [ForeignKey("User_details")]
        public int? UserDetailsId { get; set; }
    }
}
