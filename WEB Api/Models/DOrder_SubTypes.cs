using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_Api.Models
{
    public class DOrder_SubTypes
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int PlusPrice { get; set; }
        public int Count { get; set; }
        public int Orderid { get; set; }
    }
}
