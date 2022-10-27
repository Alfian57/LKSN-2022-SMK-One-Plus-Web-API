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
            var merchantList = new List<MerchantResponse>();
            var merchants = await dbContext.Merchants.OrderBy(m => m.Location).ToListAsync();

            merchants = merchants.OrderByDescending(m => m.Name).ToList();

            foreach (var merchant in merchants)
            {
                MerchantResponse item = new MerchantResponse()
                {
                    id = merchant.Id,
                    name = merchant.Name,
                    description = merchant.Description,
                    location = merchant.Location,
                    multiplier = merchant.Multiplier
                };
                merchantList.Add(item);
            }

            return Ok(merchantList);
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
            return Ok(merchant);
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
            return Ok(merchant);
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

            dbContext.Merchants.Remove(merchant);

            return Ok(merchant);
        }

    }
}
