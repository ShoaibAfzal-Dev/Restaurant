namespace MVC.Models
{
    public class CartData
    {
        public int id { get; set; }
        public string item { get; set; }
        public int price {  get; set; }
        public int quantity {  get; set; }
        public string? instructions {  get; set; }
        public List<sub_details> sub_details {  get; set; }
    }
}
