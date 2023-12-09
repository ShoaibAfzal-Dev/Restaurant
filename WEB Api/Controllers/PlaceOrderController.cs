using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using WEB_Api.Models;

namespace WEB_Api.Controllers
{
    public class PlaceOrderController : Controller
    {
        private readonly MyDbContext Db;
        public PlaceOrderController(MyDbContext _Db)
        {
            Db = _Db;
        }
        [HttpPost]
        [Route("PlaceOrder")]
        public IActionResult ReceiveDynamicData([FromBody] 
        Dictionary<string, string> formData)
        {
            try
            {
                var oi = new Order_Details
                {
                    item = formData["Name"],
                    UserId = formData["User-id"],
                    Price = int.Parse(formData["totalPrice"]),
                    Instructions = formData["Instruction"],
                    quantity =int.Parse(formData["Qty"]),
                    Status=false
                 };
                Db.Order_Details.Add(oi);
                Db.SaveChanges();

                var d= oi.Id;

                List<Order_SubDetails> orderSubDetailsList = 
                    new List<Order_SubDetails>();

                var formDataList = formData.ToList();

                for (int i = 0; i < formDataList.Count - 1; i++)
                {
                    var kvp = formDataList[i];
                    var nextKvp = formDataList[i + 1];

                    if (kvp.Key.StartsWith("Addition_") && 
                        nextKvp.Key.StartsWith("AdditionPrice_"))
                    {
                        var values = kvp.Value.Split(":");
                        var priceValues =int.Parse(nextKvp.Value);
                        Order_SubDetails ord=new Order_SubDetails()
                        {
                            details = values[0],
                            price = priceValues,
                            Order_DetailsId=oi.Id
                        };
                        orderSubDetailsList.Add(ord);

                        i++;
                    }
                }
                Console.WriteLine(orderSubDetailsList);

                Db.Order_SubDetails.AddRange(orderSubDetailsList);
                Db.SaveChanges(true);

                return Json("Data Added Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, $"Internal server error: " +
                    $"{ex.Message}");
            }
        }
        [HttpGet]
        [Route("GetOrders")]
        public IActionResult getordr(string id)
        {
            try
            {
                var order_data = from order in Db.Order_Details.
                                 Where(s => s.UserId == id && s.Status==false)
                                 join sub_order in     
                                 Db.Order_SubDetails on order.Id equals
                                 sub_order.Order_DetailsId
                                 into newprd
                                 select new
                                 {
                                     Id = order.Id,
                                     item = order.item,
                                     Price = order.Price,
                                     quantity = order.quantity,
                                     Instructions = order.Instructions,
                                     sub_details = newprd.Select(p => new
                                     {
                                         Id = p.Id,
                                         details = p.details,
                                         price = p.price,
                                         Order_DetailsId = p.Order_DetailsId,
                                     })
                                 };
                return Ok(order_data);
            }
            catch {
                return StatusCode(500,"Internal Server Error");
            }
        }
        [HttpDelete]
        [Route("DeleteData")]
        public IActionResult delete(int id)
        {
            try
            {
                var data = Db.Order_Details.FirstOrDefault(s=>s.Id==id);
                if (data != null)
                {
                    var sub_data = Db.Order_SubDetails.Where(s=>s.Order_DetailsId==id);
                    Db.Order_Details.Remove(data);
                    Db.Order_SubDetails.RemoveRange(sub_data);
                    Db.SaveChanges();
                    return Ok("Data deleted successfully");
                }
                return BadRequest("Data not exist at this Id");
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("finalize-Order")]
        public async Task<IActionResult> finalize([FromBody] 
        Dictionary<string,string> formdata)
        {
            try
            {
                var order_day = formdata["Order Day:"];
                var order_time = formdata["Order Time:"];
                var id = formdata["ids"];
                var idList = id.Split(',').Select(int.Parse).ToList();
                var payment_method = formdata["PaymentMethod"];
                   
                var user_details = new User_details()
                {
                    FirstName = formdata["First Name:"],
                    LastName = formdata["Last Name:"],
                    Email = formdata["Email:"],
                    Phone = formdata["Telephone:"],
                    Day = order_day,
                    time = order_time,
                    Card = payment_method.Equals("Card",
                    StringComparison.OrdinalIgnoreCase),
                    Cash = payment_method.Equals("Cash",
                    StringComparison.OrdinalIgnoreCase),

                };
                Db.User_details.Add(user_details);
                Db.SaveChanges();
                var ds = user_details.Id;
                foreach (var il in idList)
                {
                    var order_data = Db.Order_Details.FirstOrDefault(s => s.Id == il);
                    if (order_data != null)
                    {
                        order_data.User_detailsId = ds;
                        order_data.Status = true;
                        Db.Order_Details.Update(order_data);
                    }
                }
                Db.SaveChanges();
                return Json(new { Message = "Your data is updated successfully" });
            }
            catch {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet]
        [Route("PlacedOrder")]
        public IActionResult placedOrder()
        {
            try
            {
                var order_data = Db.Order_Details
                .Where(order => order.Status == true)
                .AsEnumerable()
                .Select(order => new
                {
                    order.Id,
                    order.item,
                    order.Price,
                    order.OrderStatus,
                    order.quantity,
                    order.Instructions,
                    sub_details = Db.Order_SubDetails
                        .Where(sub_order => sub_order.Order_DetailsId == order.Id)
                        .Select(sub_order => new
                        {
                            sub_order.Id,
                            sub_order.details,
                            sub_order.price,
                            sub_order.Order_DetailsId,
                        }),
                    user_details = Db.User_details
                        .Where(user => user.Id == order.User_detailsId)
                        .Select(user => new
                        {
                            user.Id,
                            user.FirstName,
                            user.LastName,
                            user.Email,
                            user.Phone,
                            user.Day,
                            user.Card,
                            user.Cash
                        })
            });


                return Ok(order_data);
            }
            catch
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
