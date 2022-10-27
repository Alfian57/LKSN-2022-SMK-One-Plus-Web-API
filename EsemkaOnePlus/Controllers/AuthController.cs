using EsemkaOnePlus.Data;
using EsemkaOnePlus.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EsemkaOnePlus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        public readonly IConfiguration configuration;

        public AuthController(AppDbContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var customer = await dbContext.Customers.Where(c => c.Email == request.email).FirstOrDefaultAsync();

            if (customer == null)
            {
                return NotFound();
            }

            if (customer.Password == request.password)
            {
                DateTime expired = DateTime.Now.AddMinutes(10);

                return Ok(new LoginResponse()
                {
                    token = createToken(customer, expired),
                    expiresAt = expired
                });
            } else
            {
                return NotFound();
            }
        }

        private string createToken(Customer customer, DateTime expired)
        {
            string role = string.Empty;
            if (customer.Role == 0)
            {
                role = "Member";
            } else
            {
                role = "Admin";
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, customer.Email),
                new Claim(ClaimTypes.Name, customer.Name),
                new Claim(ClaimTypes.Role, role),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                expires: expired,
                claims: claims,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
