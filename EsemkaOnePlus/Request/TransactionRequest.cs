namespace EsemkaOnePlus.Request
{
    public class TransactionRequest
    {
        public int customerId { get; set; } 
        public int merchantId { get; set; } 
        public DateTime transactionDate{ get; set; } 
        public int price { get; set; } 
    }
}
