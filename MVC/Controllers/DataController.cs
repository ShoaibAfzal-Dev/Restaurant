using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using MVC.Models;
using System;

namespace MVC.Controllers
{
    public class DataController : Controller
    {
        private readonly HttpClient _httpClient;
        // add your url
        private string url = "";
        public DataController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(url);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new
                System.Net.Http.Headers.MediaTypeWithQualityHeaderValue
                ("application/json"));
        }
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Roles") == "Admin")
            {
            HttpResponseMessage responseMessage =
                await _httpClient.GetAsync(url + "Home/GetProduct");
            if (responseMessage.IsSuccessStatusCode)
            {
                var data = await responseMessage.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var newsList = JsonSerializer.Deserialize<List<Category>>
                        (data, options);
                return View(newsList);
            }
            return View();
            }
            else
            {
                return RedirectToAction("index","home");
            }
        }
        public async Task<IActionResult> OrdersData()
        {
            if (HttpContext.Session.GetString("Roles") == "Admin")
            {
                HttpResponseMessage responseMessage =
                await _httpClient.GetAsync(url + "PlacedOrder");
            if (responseMessage.IsSuccessStatusCode)
            {
                var data = await responseMessage.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var newsList = JsonSerializer.Deserialize<List<Order_data>>
                        (data, options);
                return View(newsList);
            }
            return View();
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }
        public IActionResult Category()
        {
            if (HttpContext.Session.GetString("Roles") == "Admin")
            {
                return View();
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {
                if (HttpContext.Session.GetString("Roles") == "Admin")
                {
                    HttpResponseMessage responseMessage =
               await _httpClient.PostAsJsonAsync(url +
               $"Home/AddCategory", category);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("index", "data");
            }
            string errorMessage = await responseMessage.Content.
                    ReadAsStringAsync();
            ViewBag.Error = errorMessage;
            return View("Category");
            }
            else
            {
                return RedirectToAction("index", "home");
            }

        }
        public async Task<IActionResult> EditCategory(int id)
        {
             if (HttpContext.Session.GetString("Roles") == "Admin")
             {
                 HttpResponseMessage responseMessage =
               await _httpClient.GetAsync(url + 
               $"Home/SingleCategory?id={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var data = await responseMessage.Content.
                        ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var newsList = JsonSerializer.Deserialize
                        <Category>(data, options);
                return View(newsList);
            }
            return View();
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> SaveEditCategory
            (Category category)
        {
           if (HttpContext.Session.GetString("Roles") == "Admin")
           {
               HttpResponseMessage responseMessage =
               await _httpClient.PutAsJsonAsync(url +
               $"Home/UpdateCategory", category);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("index", "data");
            }
            return RedirectToAction("index", "data");
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (HttpContext.Session.GetString("Roles") == "Admin")
            {
                HttpResponseMessage responseMessage =
              await _httpClient.DeleteAsync(url + 
              $"Home/Deletecategory?id={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("index", "data");
            }
            return RedirectToAction("index", "data");
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }
        public async Task<IActionResult> EditProduct(int id)
        {
        if (HttpContext.Session.GetString("Roles") == "Admin")
        {
            HttpResponseMessage responseMessage =
               await _httpClient.GetAsync(url + 
               $"Home/SingleProduct?id={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var data = await responseMessage.Content
                        .ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var newsList = JsonSerializer.Deserialize
                        <products>(data, options);
                return View(newsList);
            }
            return RedirectToAction("index", "Data");
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }
        public async Task<IActionResult> SaveEditProduct(products pd)
        {
          if (HttpContext.Session.GetString("Roles") == "Admin")
          {
              HttpResponseMessage responseMessage =
               await _httpClient.PutAsJsonAsync(url +
               $"Home/UpdateProduct", pd);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("index", "data");
            }
            return RedirectToAction("index", "data");
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (HttpContext.Session.GetString("Roles") == "Admin")
            {
                HttpResponseMessage responseMessage =
              await _httpClient.DeleteAsync(url + 
              $"Home/DeleteProduct?id={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("index", "data");
            }
            return RedirectToAction("index", "data");
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }
        public IActionResult AddProducts(int id)
        {
                if (HttpContext.Session.GetString("Roles") == "Admin")
                {
                    ViewBag.id = id;
            return View();
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(products pd)
        {
                    if (HttpContext.Session.GetString("Roles") == "Admin")
                    {
                        HttpResponseMessage responseMessage =
               await _httpClient.PostAsJsonAsync(url +
               $"Home/AddProducts", pd);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("index", "data");
            }
            string errorMessage = await responseMessage.
                    Content.ReadAsStringAsync();
            ViewBag.Error = errorMessage;
            return View("AddProducts");
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }
        public async Task<IActionResult> Productdetails(int id)
        {
            if (HttpContext.Session.GetString("Roles") == "Admin")
            {
                HttpResponseMessage responseMessage =
               await _httpClient.GetAsync(url + 
               $"Order/GetOrderType?pdi={id}");
            ViewBag.id = id;
            if (responseMessage.IsSuccessStatusCode)
            {
                var data = await responseMessage.Content.
                        ReadAsStringAsync();
                var options = new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true };
                var newsList = JsonSerializer.Deserialize
                        <List<Order>>(data, options);
                return View(newsList);
            }
            ViewBag.Data = "Empty";
            return View();
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }
        public IActionResult AddNewData(int id)
        {
         if (HttpContext.Session.GetString("Roles") == "Admin")
         {
             ViewBag.id = id;
            return View();
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostData(Main_Order mo)
        {
         if (HttpContext.Session.GetString("Roles") == "Admin")
         {
             HttpResponseMessage responseMessage =
               await _httpClient.PostAsJsonAsync(url +
               $"Order/OrderOptions", mo);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Productdetails", 
                    new { id = mo.ProductId });
            }
            string errorMessage = await responseMessage.
                    Content.ReadAsStringAsync();
            ViewBag.Error = errorMessage;
            return RedirectToAction("AddNewData");
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }
        public async Task<IActionResult> EditOrder(int id)
        {
          if (HttpContext.Session.GetString("Roles") == "Admin")
          {
              HttpResponseMessage responseMessage =
               await _httpClient.GetAsync(url + 
               $"Order/GetSinglrOrderType?id={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var data = await responseMessage.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var newsList = JsonSerializer.Deserialize
                        <Main_Order>(data, options);
                return View(newsList);
            }
            return View();
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }
        public async Task<IActionResult> SaveEditOrder(Main_Order mo)
        {
            if (HttpContext.Session.GetString("Roles") == "Admin")
            {
                HttpResponseMessage responseMessage =
                   await _httpClient.PatchAsJsonAsync(url +
                   $"Order/updateTypes", mo);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Productdetails", new 
                    { id = mo.ProductId });
                }

                string errorMessage = await responseMessage.
                    Content.ReadAsStringAsync();
                ViewBag.Error = errorMessage;
                return RedirectToAction("EditOrder",new {id=mo.Id});
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }
        public async Task<IActionResult> DeleteOrder(int id,int Urlid)
        {
            if (HttpContext.Session.GetString("Roles") == "Admin")
            {
                HttpResponseMessage responseMessage = await 
                    _httpClient.DeleteAsync(
                url + $"Order/DeleteOrderType?id={id}"
                );
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Productdetails", 
                    new { id = Urlid });
            }
            return RedirectToAction("Productdetails", 
                new { id = Urlid });
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }
        public async Task<IActionResult> AddSubOrder(int id,int Urlid)
        {
          if (HttpContext.Session.GetString("Roles") == "Admin")
          {
              ViewBag.Id = id;
            ViewBag.Urlid = Urlid;
            return View();
            }
            else
            {
                return RedirectToAction("index", "home");
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddSubTypes(Order_Types Ot)
        {
         if (HttpContext.Session.GetString("Roles") == "Admin")
         {
             var Oti=new Order_Types()
            {
                Name = Ot.Name,
                PlusPrice = Ot.PlusPrice,
                Count = Ot.Count,
                Orderid = Ot.Orderid,
            };
            HttpResponseMessage responseMessage =
                   await _httpClient.PostAsJsonAsync(url +
                   $"Order/Add_Sub_Types", Oti);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Productdetails", 
                    new { id = Ot.Id });
            }

            string errorMessage = await responseMessage.
                    Content.ReadAsStringAsync();
            ViewBag.Error = errorMessage;
            return RedirectToAction("AddSubOrder", new 
            {id=Ot.Orderid ,Urlid = Ot.Id });
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }
        public async Task<IActionResult> EditSubOrder(int id,int Urlid)
        {
          if (HttpContext.Session.GetString("Roles") == "Admin")
          {
              HttpResponseMessage responseMessage = 
                    await _httpClient.GetAsync(
                            url + $"Order/Get_All_Subtypes?id={id}"
                            );
            if (responseMessage.IsSuccessStatusCode)
            {
                var data = await responseMessage.Content.
                        ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                ViewBag.main = Urlid;
                var newsList = JsonSerializer.Deserialize
                        <Order_Types>(data, options);
                return View(newsList);
            }
            return View();
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }
//        Update_SubdataTypes
       public async Task<IActionResult> SaveEditSubOrder(Order_Types order)
        {
           if (HttpContext.Session.GetString("Roles") == "Admin")
           {
               HttpResponseMessage responseMessage =
                   await _httpClient.PatchAsJsonAsync(url +
                   $"Order/Update_SubdataTypes", order);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Productdetails", new { id = order.Orderid });
            }

            string errorMessage = await responseMessage.Content.ReadAsStringAsync();
            ViewBag.Error = errorMessage;
            return RedirectToAction("Productdetails", new { id = order.Orderid });
            }
            else
            {
                return RedirectToAction("index", "home");
            }

        }
        public async Task<IActionResult> DeleteSubOrder(int id,int Urlid)
        {
           if (HttpContext.Session.GetString("Roles") == "Admin")
           {
               HttpResponseMessage responseMessage =
              await _httpClient.DeleteAsync(url + $"Order/Delete_SubOrders?id={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Productdetails", new { id = Urlid });
            }
            return RedirectToAction("Productdetails", new { id = Urlid });
            }
            else
            {
                return RedirectToAction("index", "home");
            }

        }
        public async Task<IActionResult> Verification_Mail(int id)
        {
            if (HttpContext.Session.GetString("Roles") == "Admin")
            {
                HttpResponseMessage rsm = await _httpClient.PostAsync
                    (url+$"Mail/Emailsending?id={id}",null);
                if(rsm.IsSuccessStatusCode)
                {
                    return RedirectToAction("OrdersData", "Data");

                }
                return RedirectToAction("OrdersData","Data");
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }
    }
}
