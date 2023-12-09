using Azure.Core.GeoJson;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WEB_Api.Models;

namespace WEB_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private readonly MyDbContext Db;
        public OrderController(MyDbContext _Db)
        {
            Db = _Db;
        }
        // create order type
        [HttpPost]
        [Route("OrderOptions")]
        public IActionResult OrderOptions( DOrder_types ot)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var pi=Db.Products.FirstOrDefault(s=>s.Id == ot.ProductId);
                    if (pi == null)
                    {
                        return BadRequest("product not exist ");
                    }
                    var order=new Order_Types()
                    {
                        Name = ot.Name,
                        ProductId = ot.ProductId,
                        Products=pi
                    };
                    Db.Order_types.Add(order);
                    Db.SaveChanges();
                    return Ok(order);
                }
                return BadRequest("invalid model state");
            }
            catch  { 
            return StatusCode(500,"Internal Server Error");
            }
        }
        // get order types
        [HttpGet]
        [Route("GetOrderType")]
        public IActionResult OrderOptions(int pdi)
        {
            try           {
               // var pd=Db.Order_types.Where(s=>s.ProductId== pdi).ToList();
               // var df = Db.Order_SubTypes.Where(s => s.Orderid == id);
               var gf = from order in Db.Order_types.Where(s => s.ProductId == pdi)
                      join sub_order in Db.Order_SubTypes
                      on order.Id equals sub_order.Orderid into kjh
                      select new{
                          Id=order.Id,
                          Name=order.Name,
                          Order_SubTypes=kjh.Select(p=>new
                          {
                              Id=p.Id,
                              Name=p.Name,
                              PlusPrice=p.PlusPrice,
                              Count=p.Count,
                              Orderid=p.Orderid
                          })
                      };
                if (!gf.Any())
                {
                    return BadRequest("No types in this Id");
                }
                return Ok(gf);
            }
            catch
            {
                return StatusCode(500, "Internal server Error");
            }
        }
        [HttpGet]
        [Route("GetSinglrOrderType")]
        public IActionResult SingleOrderOptions(int id)
        {
            try
            {
                 var pd=Db.Order_types.FirstOrDefault(s=>s.Id== id);
                // var df = Db.Order_SubTypes.Where(s => s.Orderid == id);
               
                if (pd==null)
                {
                    return BadRequest("No types in this Id");
                }
                return Ok(pd);
            }
            catch
            {
                return StatusCode(500, "Internal server Error");
            }
        }
        // update order types 
        [HttpPatch]
        [Route("updateTypes")]
        public IActionResult UpdateTypes([FromBody] DOrder_types ot)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var pi = Db.Order_types.FirstOrDefault(s => s.Id == ot.Id);
                    if (pi == null) { return BadRequest("No Id exist in database"); }
                   pi.Name= ot.Name;
                    Db.Order_types.Update(pi);
                    Db.SaveChanges();
                    return Ok(pi);
                }
                return BadRequest("Invalid model state");
            }
            catch
            {
                return StatusCode(500,"Internal Server Error");
            }
        }
        // delete order type
        [HttpDelete]
        [Route("DeleteOrderType")]
        public IActionResult deleteorder(int id)
        {
            try
            {
                var ds= Db.Order_types.FirstOrDefault(s=>s.Id == id);
                if (ds == null)
                {
                    return BadRequest(" data does not exist");
                }
                Db.Order_types.Remove(ds);
                Db.SaveChanges();
                return Ok(ds);
            }
            catch
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        // add order sub types
        [HttpPost]
        [Route("Add_Sub_Types")]
        public IActionResult AddSubTypes([FromBody] DOrder_SubTypes ost)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var fd = Db.Order_types.FirstOrDefault(s => s.Id == ost.Orderid);
                    if(fd == null) { return BadRequest("Data not exist on this Id"); }
                    var osty = new Order_SubTypes
                    {
                        Name= ost.Name,
                        PlusPrice=ost.PlusPrice,
                        Count=ost.Count,
                        Orderid = ost.Orderid,
                    };
                    Db.Order_SubTypes.Add(osty);
                    Db.SaveChanges();
                    return Ok(osty);

                }
                return BadRequest("invalid model state");
            }
            catch
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        // get all subtypes 
        [HttpGet]
        [Route("Get_All_Subtypes")]
        public IActionResult GetTypes(int id)
        {
            try
            {
                var df=Db.Order_SubTypes.FirstOrDefault(s=>s.Id== id);
                if (df == null)
                {
                    return BadRequest("No data exist on this request");
                }
                return Ok(df);

            }
            catch
            {
                return StatusCode(500, "Internal server Error");
            }
        }
        // delete all sub types 
        [HttpDelete]
        [Route("Delete_SubOrders")]
        public IActionResult Delete(int id)
        {
            try
            {
                var df = Db.Order_SubTypes.FirstOrDefault(s => s.Id == id);
                if (df==null){
                    return BadRequest("No data exist on this request");
                   }
                Db.Order_SubTypes.Remove(df);
                Db.SaveChanges();
                return Ok("Deleted Successfully");
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }
        // update subdata types
        [HttpPatch]
        [Route("Update_SubdataTypes")]
        public IActionResult updateTypes([FromBody] DOrder_SubTypes ef)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var sd=Db.Order_SubTypes.FirstOrDefault(s=>s.Id== ef.Id);
                    if(sd == null) {  return BadRequest("Data not exist on this request"); }
                        sd.Id = sd.Id;
                        sd.Name = ef.Name;
                        sd.PlusPrice = ef.PlusPrice;
                        sd.Count = ef.Count;
                        sd.Orderid = sd.Orderid;
                    
                    Db.Order_SubTypes.Update(sd);
                    Db.SaveChanges();
                    return Ok(sd);
                
                }
                return BadRequest("Invalid Model State");
            }
            catch {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
