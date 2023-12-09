using System.ComponentModel.DataAnnotations;

namespace WEB_Api.Models
{
    public class Coupon
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}