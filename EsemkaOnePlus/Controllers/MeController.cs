using EsemkaOnePlus.Data;
using EsemkaOnePlus.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

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
            var customerResponse = await (from c in dbContext.Customers
                                          join l in dbContext.Loyalties
                                          on c.LoyaltyId equals l.Id
                                          where c.Email == User.FindFirstValue(ClaimTypes.Email)
                                          select new
                                          {
                                              email = c.Email,
                                              name = c.Name,
                                              gender = c.Gender == 0 ? "Laki-laki" : "Perempuan",
                                              dateOfBirth = c.DateOfBirth,
                                              phoneNumber = c.PhoneNumber,
                                              address = c.Address,
                                              loyaltyName = l.Name,
                                              Point = c.TotalPoint
                                          }).FirstOrDefaultAsync();
            if (customerResponse == null)
            {
                return Unauthorized();
            }

            return Ok(customerResponse);
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

            var customerResponse = await (from c in dbContext.Customers
                                          join l in dbContext.Loyalties
                                          on c.LoyaltyId equals l.Id
                                          where c.Id == customer.Id
                                          select new
                                          {
                                              email = c.Email,
                                              name = c.Name,
                                              gender = c.Gender == 0 ? "Laki-laki" : "Perempuan",
                                              dateOfBirth = c.DateOfBirth,
                                              phoneNumber = c.PhoneNumber,
                                              address = c.Address,
                                              loyaltyName = l.Name,
                                              Point = c.TotalPoint
                                          }).FirstOrDefaultAsync();

            return Ok(customerResponse);
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

            if (postPhotoRequest.fromFile.FileName != null)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string imageFileName = DateTime.Now.Ticks.ToString() + postPhotoRequest.fromFile.FileName;
                using (FileStream fileStream = System.IO.File.Create(path + imageFileName))
                {
                    postPhotoRequest.fromFile.CopyTo(fileStream);
                    await fileStream.FlushAsync();

                    customer.PhotoPath = imageFileName;
                    await dbContext.SaveChangesAsync();
                }
            }



            return Ok();
        }
    }
}
