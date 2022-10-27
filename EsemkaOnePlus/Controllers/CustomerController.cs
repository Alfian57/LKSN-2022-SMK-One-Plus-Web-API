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
            List<CustomerResponse> customerResponses = new List<CustomerResponse>();
            var customers = await dbContext.Customers
                .ToListAsync();

            customers = customers.Where(c => c.Email.Contains(filter) | c.PhoneNumber.Contains(filter) | c.Address.Contains(filter)).ToList();
            customers.OrderBy(c => c.Name).ToList();

            foreach(var customer in customers)
            {
                CustomerResponse customerResponse = new CustomerResponse();
                customerResponse.name = customer.Name;
                customerResponse.email = customer.Email;
                customerResponse.gender = customer.Gender;
                customerResponse.dateOfBirth = customer.DateOfBirth;
                customerResponse.phoneNumber = customer.PhoneNumber;
                customerResponse.address = customer.Address;

                customerResponses.Add(customerResponse);
            }

            return Ok(customerResponses);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCustoer([FromRoute] int id)
        {
            var customer = await dbContext.Customers.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (customer == null)
            {
                return NotFound();
            }

            CustomerResponse customerResponse = new CustomerResponse();
            customerResponse.name = customer.Name;
            customerResponse.email = customer.Email;
            customerResponse.gender = customer.Gender;
            customerResponse.dateOfBirth = customer.DateOfBirth;
            customerResponse.phoneNumber = customer.PhoneNumber;
            customerResponse.address = customer.Address;

            return Ok(customerResponse);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCustoer(CreateCustomer request)
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

            CustomerResponse customerResponse = new CustomerResponse();
            customerResponse.name = customer.Name;
            customerResponse.email = customer.Email;
            customerResponse.gender = customer.Gender;
            customerResponse.dateOfBirth = customer.DateOfBirth;
            customerResponse.phoneNumber = customer.PhoneNumber;
            customerResponse.address = customer.Address;

            return Ok(customerResponse);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditCustomer(CreateCustomer request, [FromRoute] int id)
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

            CustomerResponse customerResponse = new CustomerResponse();
            customerResponse.name = customer.Name;
            customerResponse.email = customer.Email;
            customerResponse.gender = customer.Gender;
            customerResponse.dateOfBirth = customer.DateOfBirth;
            customerResponse.phoneNumber = customer.PhoneNumber;
            customerResponse.address = customer.Address;

            return Ok(customerResponse);
        }
    }
}
