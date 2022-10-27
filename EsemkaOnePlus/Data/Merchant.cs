using System;
using System.Collections.Generic;

namespace EsemkaOnePlus.Data
{
    public partial class Merchant
    {
        public Merchant()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Location { get; set; } = null!;
        public int Multiplier { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
