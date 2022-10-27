using EsemkaOnePlus.Data;
using EsemkaOnePlus.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EsemkaOnePlus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public MeController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            var customer = await dbContext.Customers.Where(c => c.Email == User.FindFirstValue(ClaimTypes.Email)).FirstOrDefaultAsync();

            if (customer == null)
            {
                return Unauthorized();
            }

            CustomerResponse response = new CustomerResponse()
            {
                name = customer.Name,
                email = customer.Email,
                gender = customer.Gender,
                dateOfBirth = customer.DateOfBirth,
                phoneNumber = customer.PhoneNumber,
                address = customer.Address,
            };

            return Ok(response);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> EditUser(CustomerRequest request)
        {
            var customer = await dbContext.Customers.Where(c => c.Email == User.FindFirstValue(ClaimTypes.Email)).FirstOrDefaultAsync();

            if (customer == null)
            {
                return Unauthorized();
            }

            customer.Name = customer.Name;
            customer.Email = customer.Email;
            customer.Gender= customer.Gender;
            customer.DateOfBirth = customer.DateOfBirth;
            customer.PhoneNumber = customer.PhoneNumber;
            customer.Address = customer.Address;

            await dbContext.SaveChangesAsync();

            CustomerResponse response = new CustomerResponse()
            {
                name = customer.Name,
                email = customer.Email,
                gender = customer.Gender,
                dateOfBirth = customer.DateOfBirth,
                phoneNumber = customer.PhoneNumber,
                address = customer.Address,
            };

            return Ok(response);
        }

        [HttpPost("Photo")]
        [Authorize]
        public async Task<IActionResult> AddPhoto([FromForm] PostPhotoRequest postPhotoRequest)
        {
            string path = Directory.GetCurrentDirectory() + @"\image\";
            var customer = await dbContext.Customers.Where(c => c.Email == User.FindFirstValue(ClaimTypes.Email)).FirstOrDefaultAsync();

            if (customer == null)
            {
                return Unauthorized();
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            

            return Ok();
        }
    }
}
