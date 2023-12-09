using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_Api.Models
{
    public class DOrder_types
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int ProductId { get; set; }
    }
}
