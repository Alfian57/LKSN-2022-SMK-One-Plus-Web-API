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
        public async Task<IActionResult> GetTrade()
        {
            List<TradeResponse> tradeResponses = new List<TradeResponse>();
            var trades = await (from t in dbContext.Trades
                                join c in dbContext.Customers
                                on t.CustomerId equals c.Id
                                join v in dbContext.Vouchers
                                on t.VoucherId equals v.Id
                                select new
                                {
                                    Id = t.Id,
                                    CusName = c.Name,
                                    VocName = v.Name,
                                    VocCode = v.Code,
                                    CreatedAt = t.CreatedAt
                                }).ToListAsync();

            trades = trades.OrderByDescending(t => t.CreatedAt).ToList();

            foreach(var trade in trades)
            {
                TradeResponse tradeResponse = new TradeResponse();
                tradeResponse.id = trade.Id;
                tradeResponse.customerName = trade.CusName;
                tradeResponse.voucherName = trade.VocName;
                tradeResponse.voucherCode = trade.VocCode;
                tradeResponse.createdAt = trade.CreatedAt;

                tradeResponses.Add(tradeResponse);
            }

            return Ok(tradeResponses);
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
                                where t.Id == id
                                select new
                                {
                                    Id = t.Id,
                                    CusName = c.Name,
                                    VocName = v.Name,
                                    VocCode = v.Code,
                                    IssuedDate = t.CreatedAt
                                }).FirstOrDefaultAsync();
            if (trade == null) return NotFound();

            TradeResponse tradeResponse = new TradeResponse();
            tradeResponse.id = trade.Id;
            tradeResponse.customerName = trade.CusName;
            tradeResponse.voucherName = trade.VocName;
            tradeResponse.voucherCode = trade.VocCode;
            tradeResponse.createdAt = trade.IssuedDate;

            return Ok(tradeResponse);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCustoer(TradeRequest request)
        {
            var voucher =await dbContext.Vouchers.Where(v => v.Id == request.voucherId).FirstOrDefaultAsync();
            var customer =await dbContext.Customers.Where(v => v.Id == request.customerId).FirstOrDefaultAsync();

            if (voucher == null) return BadRequest("Vouvher Id Not Valid");
            if (customer == null) return BadRequest("Customer Id Not Valid");

            Trade trade = new Trade();
            TradeResponse tradeResponse = new TradeResponse();

            trade.CustomerId = request.customerId;
            trade.VoucherId = request.voucherId;
            trade.CreatedAt = DateTime.Now;

            await dbContext.AddAsync(trade);
            await dbContext.SaveChangesAsync();

            var trade2 = await (from t in dbContext.Trades
                               join c in dbContext.Customers
                               on t.CustomerId equals c.Id
                               join v in dbContext.Vouchers
                               on t.VoucherId equals v.Id
                               where t.Id == trade.Id
                               select new
                               {
                                   Id = t.Id,
                                   CusName = c.Name,
                                   VocName = v.Name,
                                   VocCode = v.Code,
                                   IssuedDate = t.CreatedAt
                               }).FirstOrDefaultAsync();

            tradeResponse.id = trade.Id;
            tradeResponse.customerName = trade2.CusName;
            tradeResponse.voucherName = trade2.VocName;
            tradeResponse.voucherCode = trade2.VocCode;
            tradeResponse.createdAt = trade2.IssuedDate;
            return Ok(tradeResponse);
        }
    }
}
