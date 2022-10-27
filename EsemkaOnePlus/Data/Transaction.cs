using System;
using System.Collections.Generic;

namespace EsemkaOnePlus.Data
{
    public partial class Transaction
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int MerchantId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Price { get; set; }
        public decimal Point { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual Merchant Merchant { get; set; } = null!;
    }
}
