using System;
using System.Collections.Generic;

namespace EsemkaOnePlus.Data
{
    public partial class Loyalty
    {
        public Loyalty()
        {
            Customers = new HashSet<Customer>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal RequiredPoint { get; set; }
        public int Multiplier { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
