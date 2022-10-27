using System;
using System.Collections.Generic;

namespace EsemkaOnePlus.Data
{
    public partial class Voucher
    {
        public Voucher()
        {
            Trades = new HashSet<Trade>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Cost { get; set; }
        public int Limit { get; set; }
        public DateTime ActivatedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Trade> Trades { get; set; }
    }
}
