namespace EsemkaOnePlus.Request
{
    public class MerchantRequest
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public int multiplier { get; set; }
        public DateTime createdAt { get; set; }
    }
}
