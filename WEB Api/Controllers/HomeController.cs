using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_Api.Models;

namespace WEB_Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Admin")]
    public class HomeController : ControllerBase
    {
        private readonly MyDbContext Db;
        public HomeController(MyDbContext _Db)
        {
            Db = _Db;
        }
        // add a new category
        [HttpPost]
        [Route("AddCategory")]
        public IActionResult AddCategory([FromBody] Category ct)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var chk = Db.Category.FirstOrDefault(s => s.Name.ToUpper() == ct.Name.ToUpper());
                    if (chk != null)
                    {
                        return BadRequest("Category already exists");
                    }
                    var df = new Category
                    {
                        Name = ct.Name.ToUpper(),
                        Description = ct.Description,
                    };
                    Db.Category.Add(df);
                    Db.SaveChanges();
                    return Ok(df);
                }
                return BadRequest(ModelState);
            }
            catch
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        // add products to the category
        [HttpPost]
        [Route("AddProducts")]
        public IActionResult addproduct([FromBody] Prdts pd)
        {
            try
            {
                var chk = Db.Category.FirstOrDefault(s => s.Id == pd.CategoryId);
                if (chk == null)
                {
                    return BadRequest("Category did not exists");
                }
                var prd = new Products()
                {
                    Name = pd.Name,
                    Description = pd.Description,
                    price = pd.price,
                    Quantity = pd.Quantity,
                    Categoryid = chk.Id
                };
                Db.Products.Add(prd);
                Db.SaveChanges();
                return Ok(prd);
            }
            catch
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        // get products with respective of their category
        [HttpGet]
        [Route("GetProduct")]
        public IActionResult getproduct()
        {
            try
            {
                var query = from category in Db.Category
                            join product in Db.Products on category.Id equals product.Categoryid into categoryProducts
                            select new
                            {
                                Id = category.Id,
                                Name = category.Name,
                                Description = category.Description,
                                Products = categoryProducts.Select(p => new
                                {
                                    Id = p.Id,
                                    Name = p.Name,
                                    Description = p.Description,
                                    price=  p.price,
                                    Quantity=p.Quantity
                                })
                            };
                var result = query.ToList();
                return Ok(result);
            }
            catch
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        // get all category
        [HttpGet]
        [Route("GetCategory")]
        public IActionResult getCategory()
        {
            try
            {
                var sd = Db.Category.ToList();
                return Ok(sd);

            }
            catch
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        // get single category
        [HttpGet]
        [Route("SingleCategory")]
        public IActionResult SingleCategory(int id)
        {
            try
            {
                var sd = Db.Category.FirstOrDefault(s => s.Id == id);
                if (sd == null)
                {
                    return BadRequest("No Data exist on this request");
                }
                return Ok(sd);

            }
            catch
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        // Get Single product 
        [HttpGet]
        [Route("SingleProduct")]
        public IActionResult SingleProduct(int id)
        {
            try
            {
                var sd = Db.Products.FirstOrDefault(s => s.Id == id);
                if (sd == null)
                {
                    return BadRequest("No Data exist on this request");
                }
                return Ok(sd);

            }
            catch
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        // update a category
        [HttpPut]
        [Route("UpdateCategory")]
        public IActionResult UpdateCategory([FromBody] Category ct)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var ne = new Category()
                    {
                        Id = ct.Id,
                        Name = ct.Name.ToUpper(),
                        Description = ct.Description,
                    };
                    Db.Category.Update(ne);
                    Db.SaveChanges();
                    return Ok(ne);
                }
                return BadRequest("invalid State");
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }
        // update a product 
        [HttpPut]
        [Route("UpdateProduct")]
        public IActionResult UpdateProduct(Dproducts pd)
        {
            try
            {
                var existingProduct = Db.Products.FirstOrDefault(p => p.Id == pd.Id);

                if (existingProduct == null)
                {
                    return BadRequest("Product not found");
                }
                existingProduct.Name = pd.Name;
                existingProduct.Description = pd.Description;
                existingProduct.price = pd.price;
                existingProduct.Quantity = pd.Quantity;
                existingProduct.Categoryid = existingProduct.Categoryid;
                Db.Products.Update(existingProduct);
                Db.SaveChanges();

                return Ok(existingProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        // Delete product
        [HttpDelete]
        [Route("DeleteProduct")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                var pd = Db.Products.FirstOrDefault(s => s.Id == id);
                if (pd == null)
                {
                    return BadRequest("No data exist in this request");
                }
                Db.Products.Remove(pd);
                Db.SaveChanges();
                return Ok(pd);
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }
        // delete categroy and their respective products
        [HttpDelete]
        [Route("Deletecategory")]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                var ct = Db.Category.FirstOrDefault(s => s.Id == id);
                if (ct == null)
                {
                    return BadRequest("No data exist in this request");
                }
                var pd = Db.Products.Where(s => s.Categoryid == id);


                Db.RemoveRange(pd);
                Db.Remove(ct);
                Db.SaveChanges();
                return Ok(ct);
            }
            catch
            {
                return StatusCode(500, "Internal Server error");
            }
        }
    }
}