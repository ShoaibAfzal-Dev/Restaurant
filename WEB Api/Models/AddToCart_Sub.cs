using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_Api.Models
{
    public class AddToCart_Sub
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int? quantity { get; set; }
        public int? price { get; set; }
        [ForeignKey("AddToCart")]    
        public int AddToCartId { get; set; }
        /*public virtual AddToCart AddToCart { get; set; }*/
    }
}
