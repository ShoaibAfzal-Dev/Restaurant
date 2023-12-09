using Microsoft.AspNetCore.Mvc;
using WEB_Api.Models;

namespace WEB_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartUController : Controller
    {
        private readonly MyDbContext Db;
        public CartUController(MyDbContext _Db)
        {
            Db = _Db;
        }
        [HttpPost]
        [Route("UserDetails")]
        public IActionResult Index([FromForm] User_details ud)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    if(ud.Card==ud.Cash)
                    {
                        return BadRequest("You hove to chosse one payment method");
                    }
                    Db.User_details.Add(ud);
                    Db.SaveChanges();
                    return Ok(ud);
                }
                return BadRequest("Invalid Model State");
            }
            catch {
                return StatusCode(500, "Internal Server Error");
            }            
        }
        // add to cart
        [HttpPost]
        [Route("AddtoCart")]
        public IActionResult addtoCart([FromForm] AddToCart atc)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var fd = new AddToCart
                    {
                        Qty= atc.Qty,
                        item=atc.item,
                        TotalPrice=atc.TotalPrice,

                    };
                    Db.addToCarts.Add(atc);
                    Db.SaveChanges();
                    return Ok(atc);
                }
                return BadRequest("Invalid Model State");
            }
            catch
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpPost]
        [Route("AddToSubCart")]
        public IActionResult AddToSCart([FromForm] AddToCart_Sub Atsc)
        {
            try {
                if (ModelState.IsValid)
                {
                    var fid = Db.addToCarts.Where(s => s.Id == Atsc.AddToCartId);
                    if (!fid.Any()) { return BadRequest("Data not exist on cartid"); }
                    var ds = new AddToCart_Sub {
                    Title = Atsc.Title,
                    quantity=Atsc.quantity,
                    price=Atsc.price,
                    AddToCartId = Atsc.AddToCartId,
                    };
                    Db.addToCart_Subs.Add(ds);
                    Db.SaveChanges();
                    return Ok(Atsc);
                }
                return BadRequest("Invalid Model State");
            }
            catch { return StatusCode(500, "Internal Server Error"); }
        }
        [HttpPut]
        [Route("PlaceOrder")]
        public IActionResult PlaceOrder([FromForm] AddToCart AtC )
        {
            try {


                return Ok("Order Done");
            } catch { return StatusCode(500, "Internal Server Error"); }
        }
        [HttpGet]
        [Route("GetAllCart")]
        public IActionResult list()
        {
            try
            {

                var li = from cart in Db.addToCarts
                         join Scart in Db.addToCart_Subs on
                          cart.Id equals Scart.AddToCartId into subcart
                         select new
                         {
                            CartId= cart.Id,
                            CartItem=cart.item,
                            CartPrice=cart.TotalPrice,
                            CartQty=cart.Qty,
                            Subcart=subcart.Select(s=>new
                            {
                                SubCartId=s.Id,
                                SubCartTitle=s.Title,
                                SubCartQuantity=s.quantity,
                                SubCartPrice=s.price,
                            })
                         };
                if (li.Any())
                {
                    return Ok(li);
                }
                return Ok("No Data exist");
            }
            catch
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpDelete]
        [Route("DeleteCart")]
        public IActionResult delScart(int Id)
        {
            try
            {
                var fid = Db.addToCarts.FirstOrDefault(s=>s.Id==Id);
                if(fid == null) { return BadRequest("Data Not Exist"); }
                var subItems = Db.addToCart_Subs.Where(s => s.AddToCartId == Id);
                Db.addToCart_Subs.RemoveRange(subItems);
                Db.addToCarts.Remove(fid);
                Db.SaveChanges();
                return Ok("Cart item and associated sub-items deleted successfully");

            }
            catch
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}