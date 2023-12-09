namespace WEB_Api.Models
{
    public class Prdts
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int price { get; set; }
        public int Quantity { get; set; }
        public int? CategoryId { get; set; }
    }
}
