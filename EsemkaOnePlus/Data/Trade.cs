using System;
using System.Collections.Generic;

namespace EsemkaOnePlus.Data
{
    public partial class Trade
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int VoucherId { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual Voucher Voucher { get; set; } = null!;
    }
}
