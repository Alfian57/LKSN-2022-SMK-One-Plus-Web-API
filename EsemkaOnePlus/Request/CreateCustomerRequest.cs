namespace EsemkaOnePlus.Request
{
    public class CreateCustomerRequest
    {
        public int id { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public int gender { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string phoneNumber { get; set; }
        public string address { get; set; }
        public int role { get; set; }
        public int loyaltyId { get; set; }
        public DateTime loyaltyExpiredDate { get; set; }
        public string photoPath { get; set; }
        public int totalPoint { get; set; }
        public DateTime createdAt { get; set; }
    }
}
