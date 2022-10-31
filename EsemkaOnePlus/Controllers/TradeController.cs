using EsemkaOnePlus.Data;
using EsemkaOnePlus.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;

namespace EsemkaOnePlus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradeController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public TradeController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetTrade(string filter)
        {
            var trades = await (from t in dbContext.Trades
                                join c in dbContext.Customers
                                on t.CustomerId equals c.Id
                                join v in dbContext.Vouchers
                                on t.VoucherId equals v.Id
                                orderby t.CreatedAt ascending
                                select new
                                {
                                    Id = t.Id,
                                    customerName = c.Name,
                                    voucherName = v.Name,
                                    voucherCode = v.Code,
                                    issuedDate = t.CreatedAt
                                }).ToListAsync();

            if (filter != null)
            {
                trades = trades.Where(t => t.customerName.Contains(filter) && t.voucherName.Contains(filter)).ToList();
            }

            return Ok(trades);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetTradeDetail([FromRoute] int id)
        {
            var trade = await (from t in dbContext.Trades
                               join c in dbContext.Customers
                               on t.CustomerId equals c.Id
                               join v in dbContext.Vouchers
                               on t.VoucherId equals v.Id
                               orderby t.CreatedAt ascending
                               where t.Id == id
                               select new
                               {
                                   Id = t.Id,
                                   customerName = c.Name,
                                   voucherName = v.Name,
                                   voucherCode = v.Code,
                                   issuedDate = t.CreatedAt
                               }).FirstOrDefaultAsync();

            if (trade == null)
            {
                return NotFound();
            }

            return Ok(trade);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCustoer(TradeRequest request)
        {
            var voucher = await dbContext.Vouchers.Where(v => v.Id == request.voucherId).FirstOrDefaultAsync();
            var customer = await dbContext.Customers.Where(v => v.Id == request.customerId).FirstOrDefaultAsync();

            if (voucher == null)
            {
                return BadRequest("Voucher Id Not Valid");
            }
            if (customer == null)
            {
                return BadRequest("Customer Id Not Valid");
            }

            Trade trade = new Trade();

            trade.CustomerId = request.customerId;
            trade.VoucherId = request.voucherId;
            trade.CreatedAt = DateTime.Now;

            await dbContext.AddAsync(trade);
            await dbContext.SaveChangesAsync();

            var tradeResponse = await (from t in dbContext.Trades
                               join c in dbContext.Customers
                               on t.CustomerId equals c.Id
                               join v in dbContext.Vouchers
                               on t.VoucherId equals v.Id
                               orderby t.CreatedAt ascending
                               select new
                               {
                                   Id = t.Id,
                                   customerName = c.Name,
                                   voucherName = v.Name,
                                   voucherCode = v.Code,
                                   issuedDate = t.CreatedAt
                               }).FirstOrDefaultAsync();

            return Ok(tradeResponse);
        }
    }
}
