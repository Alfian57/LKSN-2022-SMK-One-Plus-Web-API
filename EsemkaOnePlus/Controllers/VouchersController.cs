using EsemkaOnePlus.Data;
using EsemkaOnePlus.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EsemkaOnePlus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VouchersController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public VouchersController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetVoucher()
        {
            List<VoucherResponse> responses = new List<VoucherResponse>();

            var vouchers = await dbContext.Vouchers.ToListAsync();

            vouchers = vouchers.OrderByDescending(v => v.Code).ToList();

            foreach (var voucher in vouchers)
            {
                VoucherResponse  response = new VoucherResponse();
                response.code = voucher.Code;
                response.name = voucher.Name;
                response.description = voucher.Description;
                response.cost = voucher.Cost;
                response.limit = voucher.Limit;
                response.activatedAt = voucher.ActivatedAt;
                response.expiredAt = voucher.ExpiredAt;
                response.createdAt = voucher.CreatedAt;

                responses.Add(response);
            }

            return Ok(responses);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateVoucher(VoucherRequest request)
        {
            var voucher = new Voucher();

            voucher.Code = request.code;
            voucher.Name = request.name;
            voucher.Description = request.description;
            voucher.Cost = request.cost;
            voucher.Limit = request.limit;
            voucher.ActivatedAt = request.activatedAt;
            voucher.ExpiredAt = request.expiredAt;
            voucher.CreatedAt = request.createdAt;


            await dbContext.AddAsync(voucher);
            await dbContext.SaveChangesAsync();
            return Ok(voucher);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditVoucher(VoucherRequest request, [FromRoute] int id)
        {
            var voucher = await dbContext.Vouchers.Where(v => v.Id == id).SingleOrDefaultAsync();

            if (voucher == null)
            {
                return NotFound();
            }

            voucher.Code = request.code;
            voucher.Name = request.name;
            voucher.Description = request.description;
            voucher.Cost = request.cost;
            voucher.Limit = request.limit;
            voucher.ActivatedAt = request.activatedAt;
            voucher.ExpiredAt = request.expiredAt;
            voucher.CreatedAt = request.createdAt;

            await dbContext.SaveChangesAsync();
            return Ok(voucher);
        }
    }
}
