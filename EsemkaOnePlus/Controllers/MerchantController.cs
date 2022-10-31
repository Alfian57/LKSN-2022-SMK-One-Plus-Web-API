    using EsemkaOnePlus.Data;
using EsemkaOnePlus.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EsemkaOnePlus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public MerchantController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMerchant()
        {
            var merchants = await (from m in dbContext.Merchants
                                   orderby m.Location, m.Name
                                   select new { 
                                       id = m.Id,
                                       name = m.Name,
                                       description = m.Description,
                                       location = m.Location,
                                       multiplier = m.Multiplier,
                                   }).ToListAsync();

            return Ok(merchants);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateMerchant(MerchantRequest request)
        {
            Merchant merchant = new Merchant()
            {
                Id = request.id,
                Name = request.name,
                Description = request.description,
                Location = request.location,
                Multiplier = request.multiplier,
                CreatedAt = request.createdAt,
            };

            await dbContext.AddAsync(merchant);
            await dbContext.SaveChangesAsync();

            var merchantResponse = await (from m in dbContext.Merchants
                                   orderby m.Location, m.Name
                                   where m.Id == merchant.Id
                                   select new
                                   {
                                       id = m.Id,
                                       name = m.Name,
                                       description = m.Description,
                                       location = m.Location,
                                       multiplier = m.Multiplier,
                                   }).FirstOrDefaultAsync();

            return Ok(merchantResponse);
        }

        [HttpPut()]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditMerchant(MerchantRequest request)
        {
            var merchant = await dbContext.Merchants.Where(m => m.Id == request.id).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return NotFound();
            }

            merchant.Id = request.id;
            merchant.Name = request.name;
            merchant.Description = request.description;
            merchant.Location = request.location;
            merchant.Multiplier = request.multiplier;
            merchant.CreatedAt = request.createdAt;

            await dbContext.SaveChangesAsync();

            var merchantResponse = await (from m in dbContext.Merchants
                                          orderby m.Location, m.Name
                                          where m.Id == merchant.Id
                                          select new
                                          {
                                              id = m.Id,
                                              name = m.Name,
                                              description = m.Description,
                                              location = m.Location,
                                              multiplier = m.Multiplier,
                                          }).FirstOrDefaultAsync();

            return Ok(merchantResponse);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditMerchant([FromRoute] int id)
        {
            var merchant = await dbContext.Merchants.Where(m => m.Id == id).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return NotFound();
            }

            var merchantResponse = await (from m in dbContext.Merchants
                                          orderby m.Location, m.Name
                                          where m.Id == merchant.Id
                                          select new
                                          {
                                              id = m.Id,
                                              name = m.Name,
                                              description = m.Description,
                                              location = m.Location,
                                              multiplier = m.Multiplier,
                                          }).FirstOrDefaultAsync();

            dbContext.Merchants.Remove(merchant);

            return Ok(merchantResponse);
        }

    }
}
