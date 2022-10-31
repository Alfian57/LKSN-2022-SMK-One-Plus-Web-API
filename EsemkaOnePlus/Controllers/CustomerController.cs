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
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public CustomerController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCustomer(string filter)
        {
            var customers = await (from c in dbContext.Customers
                                   join l in dbContext.Loyalties
                                   on c.LoyaltyId equals l.Id
                                   orderby c.Name, c.Email ascending
                                   select new {
                                       email = c.Email,
                                       name = c.Name,
                                       gender = c.Gender == 0 ? "Laki-laki" : "Perempuan",
                                       dateOfBirth = c.DateOfBirth,
                                       phoneNumber = c.PhoneNumber,
                                       address = c.Address,
                                       loyaltyName = l.Name,
                                       Point = c.TotalPoint
                                   }).ToListAsync();


            if (filter != null)
            {
                customers = customers.Where(c => c.email.Contains(filter) | c.name.Contains(filter) | c.phoneNumber.Contains(filter) | c.address.Contains(filter)).ToList();
            }

            return Ok(customers);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCustoer([FromRoute] int id)
        {
            var customer = await (from c in dbContext.Customers
                                   join l in dbContext.Loyalties
                                   on c.LoyaltyId equals l.Id
                                   where c.Id == id
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
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCustoer(CreateCustomerRequest request)
        {
            Customer customer = new Customer();
            var loyalty = await dbContext.Loyalties.Where(l => l.Id == request.loyaltyId).FirstOrDefaultAsync();
            if (request.role != 0 || request.role != 1)
            {
                return BadRequest("Role Not Valid");
            }
            if (request.gender != 0 || request.gender != 1)
            {
                return BadRequest("Gender Not Valid");
            }
            if (loyalty == null)
            {
                return BadRequest("Loyalty Id Not Valid");
            }
            

            customer.Email = request.email;
            customer.Name = request.name;
            customer.Password = request.password;
            customer.Gender = request.gender;
            customer.DateOfBirth = request.dateOfBirth;
            customer.PhoneNumber = request.phoneNumber;
            customer.Address = request.address;
            customer.Role = request.role;
            customer.LoyaltyId = request.loyaltyId;
            customer.LoyaltyExpiredDate = request.loyaltyExpiredDate;
            customer.PhoneNumber = request.phoneNumber;
            customer.TotalPoint = request.totalPoint;
            customer.CreatedAt = request.createdAt;

            await dbContext.AddAsync(customer);
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

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditCustomer(CreateCustomerRequest request, [FromRoute] int id)
        {
            var customer = await dbContext.Customers.Where(c => c.Id == id).FirstOrDefaultAsync();
            var loyalty = await dbContext.Loyalties.Where(l => l.Id == request.loyaltyId).FirstOrDefaultAsync();
            if (customer == null)
            {
                return NotFound();
            }
            if (request.role != 0 || request.role != 1)
            {
                return BadRequest("Role Not Valid");
            }
            if (request.gender != 0 || request.gender != 1)
            {
                return BadRequest("Gender Not Valid");
            }
            if (loyalty == null)
            {
                return BadRequest("Loyalty Id Not Valid");
            }

            customer.Email = request.email;
            customer.Name = request.name;
            customer.Password = request.password;
            customer.Gender = request.gender;
            customer.DateOfBirth = request.dateOfBirth;
            customer.PhoneNumber = request.phoneNumber;
            customer.Address = request.address;
            customer.Role = request.role;
            customer.LoyaltyId = request.loyaltyId;
            customer.LoyaltyExpiredDate = request.loyaltyExpiredDate;
            customer.PhoneNumber = request.phoneNumber;
            customer.TotalPoint = request.totalPoint;
            customer.CreatedAt = request.createdAt;

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
    }
}
