using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MVC.Controllers
{
    
    public class Orders : Controller
    {
        private readonly HttpClient _httpClient;
        private string url = "";
        public Orders()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(url);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new 
            MediaTypeWithQualityHeaderValue
                ("application/json"));
        }
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage responseMessage = 
                await _httpClient.GetAsync(url+ "Home/GetProduct");
            if(responseMessage.IsSuccessStatusCode) {
                var data = await responseMessage.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var newsList = JsonSerializer.Deserialize<List<Category>>(data, options);
                return View(newsList);
            }
            return View();
        }
        public IActionResult Info() {
            return View();
        }
        public async Task<IActionResult> Cart()
        {
            if (HttpContext.Session.GetString("jti") != null)
            {
                var id = HttpContext.Session.GetString("jti");
                HttpResponseMessage responsemessage = await
                _httpClient.GetAsync(url + $"GetOrders?id={id}");
                if (responsemessage.IsSuccessStatusCode)
                {
                    var data = await responsemessage.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    var finaldata = JsonSerializer.Deserialize<List<CartData>>(data, options);
                    return View(finaldata);
                }
            }
            ViewBag.None = 0;
            return View();
        }

    }
}
