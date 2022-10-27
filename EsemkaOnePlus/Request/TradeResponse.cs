namespace EsemkaOnePlus.Request
{
    public class TradeResponse
    {
        public int id { get; set; }
        public string customerName { get; set; }
        public string voucherCode { get; set; }
        public string voucherName{ get; set; }
        public DateTime createdAt { get; set; }
    }
}
