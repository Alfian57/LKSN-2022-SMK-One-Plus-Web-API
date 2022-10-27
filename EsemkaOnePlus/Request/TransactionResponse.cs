using EsemkaOnePlus.Data;

namespace EsemkaOnePlus.Request
{
    public class TransactionResponse
    {
        public int id { get; set; }
        public int customerId { get; set; }
        public int merchantId { get; set; }
        public DateTime transactionDate { get; set; }
        public int price{ get; set; }
        public int point { get; set; }
        public DateTime createdAt { get; set; }
        public Customer customer { get; set; }

    }
}
