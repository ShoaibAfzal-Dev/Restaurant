using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WEB_Api.Models;

namespace WEB_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        public AccountController(
            SignInManager<IdentityUser> _signInManager,
            RoleManager<IdentityRole> _roleManager,
            UserManager<IdentityUser> _userManager,
            IConfiguration _configuration){
            signInManager = _signInManager;
            roleManager = _roleManager;
            userManager = _userManager;
            configuration = _configuration;
        }
        [HttpPost]
        [Route("Register_User")]
        public async Task<IActionResult> SignUp(Register rg)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var User = new IdentityUser
                    {
                        UserName = rg.UserName,
                        Email = rg.Email,
                    };
                    var register = await userManager.CreateAsync(User, rg.Password);
                    if (register.Succeeded)
                    {
                        var AsignRole = roleManager.FindByNameAsync("User").Result;
                        if (AsignRole != null)
                        {
                            await userManager.AddToRoleAsync(User, AsignRole.Name);
                        }
                        return Ok("Registration Successfull");
                    }
                    return BadRequest(register.Errors);
                }
                return BadRequest("Invalid Model State");
            }
            catch { return StatusCode(500, "Internal Server Error"); }
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login( Login ln)
        {
            try
            {
              var user = await userManager.FindByNameAsync(ln.userName);
              if (user != null & await userManager.CheckPasswordAsync(user,ln.Password))
                   {
                    var token = GenerateToken(user);
                    return Ok(new { Token =token });
                   }
              return Unauthorized("Invalid UserName or Password");
            }
            catch
            {
                return StatusCode(500,"Internal Server Error");
            }
        } 
        private string GenerateToken(IdentityUser user)
        {
            var claims = new List<Claim>
             {
              new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
               new Claim(JwtRegisteredClaimNames.Jti, user.Id),
               
             };

            var userRoles = userManager.GetRolesAsync(user).Result;
            claims.AddRange(userRoles.Select(role => new Claim("Roles", role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:secret"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(50),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.WriteToken(token);

            return jwt;

        }
        [HttpPost]
        [Route("AddRoles")]
        public async Task addRole(string name)
        {
            var find=await roleManager.FindByNameAsync(name);
            if (find == null)
            {
                await roleManager.CreateAsync(new IdentityRole(name));
            }
        }
    }
}
