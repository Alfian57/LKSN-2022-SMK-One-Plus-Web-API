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
            var vouchers = await (from v in dbContext.Vouchers
                                  orderby v.Code ascending
                                  select new
                                  {
                                      id = v.Id,
                                      code = v.Code,
                                      name = v.Name,
                                      description = v.Description,
                                      cost = v.Cost,
                                      limit = v.Limit,
                                      activatedAt = v.ActivatedAt,
                                      expiredAt = v.ExpiredAt,
                                  }).ToListAsync();


            return Ok(vouchers);
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
            voucher.CreatedAt = DateTime.Now;

            await dbContext.AddAsync(voucher);
            await dbContext.SaveChangesAsync();

            var voucherResponse = await (from v in dbContext.Vouchers
                                  orderby v.Code ascending
                                  where v.Id == voucher.Id
                                  select new
                                  {
                                      id = v.Id,
                                      code = v.Code,
                                      name = v.Name,
                                      description = v.Description,
                                      cost = v.Cost,
                                      limit = v.Limit,
                                      activatedAt = v.ActivatedAt,
                                      expiredAt = v.ExpiredAt,
                                  }).FirstOrDefaultAsync();

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

            await dbContext.SaveChangesAsync();

            var voucherResponse = await (from v in dbContext.Vouchers
                                         orderby v.Code ascending
                                         where v.Id == voucher.Id
                                         select new
                                         {
                                             id = v.Id,
                                             code = v.Code,
                                             name = v.Name,
                                             description = v.Description,
                                             cost = v.Cost,
                                             limit = v.Limit,
                                             activatedAt = v.ActivatedAt,
                                             expiredAt = v.ExpiredAt,
                                         }).FirstOrDefaultAsync();

            return Ok(voucherResponse);
        }
    }
}
