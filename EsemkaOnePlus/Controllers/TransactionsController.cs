using EsemkaOnePlus.Data;
using EsemkaOnePlus.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace EsemkaOnePlus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public TransactionsController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetTransaction()
        {
            var transactions = await (from t in dbContext.Transactions
                                      join c in dbContext.Customers
                                      on t.CustomerId equals c.Id
                                      join m in dbContext.Merchants
                                      on t.MerchantId equals m.Id
                                      select new {
                                        CustomerName = c.Name,
                                        MerchantName = m.Name,
                                        TransactionDate = t.TransactionDate,
                                        Price = t.Price,
                                        Point = t.Point
                                      }
                                      ).ToListAsync();

            transactions = transactions.OrderByDescending(t => t.TransactionDate).ToList();

            return Ok(transactions);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTransaction(TransactionRequest request)
        {
            var customer = await dbContext.Customers.Where(c => c.Email == User.FindFirstValue(ClaimTypes.Email)).FirstOrDefaultAsync();

            if (customer == null)
            {
                return Unauthorized();
            }

            Transaction transaction = new Transaction()
            {
                CustomerId = request.customerId,
                MerchantId = request.merchantId,
                TransactionDate = request.transactionDate,
                Price = request.price,
                Point = request.price / 300
            };

            customer.TotalPoint += request.price / 300;

            await dbContext.AddAsync(transaction);
            await dbContext.SaveChangesAsync();
            return Ok(transaction);
        }

        [HttpGet("ExportCSV")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCSV()
        {
            //Customer Name,Merchant Name,Transaction Date,Points
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Customer Name, Merchant Name, Transaction Date, Point");

            var transactions = await (from t in dbContext.Transactions
                                      join c in dbContext.Customers
                                      on t.CustomerId equals c.Id
                                      join m in dbContext.Merchants
                                      on t.MerchantId equals m.Id
                                      select new
                                      {
                                          CusName = c.Name,
                                          MerName = m.Name,
                                          Date = t.TransactionDate,
                                          Point = t.Point
                                      }).ToListAsync();

            foreach (var transaction in transactions)
            {
                stringBuilder.AppendLine(transaction.CusName + ", " + transaction.MerName + ", " + transaction.Date + ", " + transaction.Point);
            }            
            return File(Encoding.UTF8.GetBytes(stringBuilder.ToString()), "text/csv", "data.csv");
        }
    }
}
