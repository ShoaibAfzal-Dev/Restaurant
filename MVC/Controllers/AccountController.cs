using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using MVC.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TokenResponse = MVC.Models.TokenResponse;

namespace MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        // add your url
        private string url = "";

        public AccountController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(url);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(Register register)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage responseMessage = await _httpClient.PostAsJsonAsync(url + "Account/Register_User", register);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return View("Login");
                }
                else
                {
                    ViewBag.error = responseMessage.Content.ReadAsStringAsync().Result;
                    return View();
                }
            }
            ViewBag.error = "Invalid Model state";
            return View(register);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage responseMessage = await _httpClient.PostAsJsonAsync(
                    url + "Account/Login", login);

                if (responseMessage.IsSuccessStatusCode)
                {
                    string json = await responseMessage.Content.ReadAsStringAsync();

                    TokenResponse tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(json);

                    string token = tokenResponse.Token;

                    HttpContext.Session.SetString("JwtToken", token);

                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                    var claims = jsonToken?.Claims;
                    foreach (var claim in claims)
                    {
                        HttpContext.Session.SetString(claim.Type, claim.Value);
                    }

                    //var jwttoken = sessionStorage.getItem('jwtToken');
                    // var role=HttpContext.Session.GetString("Roles"); 
                    // var nma=HttpContext.Session.GetString("sub");
                    // var id =HttpContext.Session.GetString("jti");
                    return RedirectToAction("Index","Home"); 
                }
                else
                {
                    ViewBag.Error = responseMessage.Content.ReadAsStringAsync().Result;
                    return View(login);
                }
            }

            ViewBag.Error = "Invalid Model State";
            return View(login);
        }
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("index","home");
        }

    }
}
