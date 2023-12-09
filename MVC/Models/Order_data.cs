namespace MVC.Models
{
    public class Order_data
    {
        public int id { get; set; }
        public bool OrderStatus {  get; set; }
        public string item { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
        public string? instructions { get; set; }
        public List<sub_details> sub_details { get; set; }
        public List<User_Details> User_Details { get; set; }
    }
}
