using System.ComponentModel.DataAnnotations;

namespace WEB_Api.Models
{
    public class BookOrder
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
    }
}
