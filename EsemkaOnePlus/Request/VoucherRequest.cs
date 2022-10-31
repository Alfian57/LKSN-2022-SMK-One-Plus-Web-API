namespace EsemkaOnePlus.Request
{
    public class VoucherRequest
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal cost { get; set; }
        public int limit { get; set; }
        public DateTime activatedAt { get; set; }
        public DateTime expiredAt { get; set; }

    }
}
