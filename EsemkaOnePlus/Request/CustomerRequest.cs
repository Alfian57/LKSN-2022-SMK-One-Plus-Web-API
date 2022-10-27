namespace EsemkaOnePlus.Request
{
    public class CustomerRequest
    {
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public int gender{ get; set; }
        public DateTime dateOfBirth { get; set; }
        public string phoneNumber { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;

    }
}
