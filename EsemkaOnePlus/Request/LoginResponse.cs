namespace EsemkaOnePlus.Request
{
    public class LoginResponse
    {
        public string token { get; set; } = string.Empty;
        public DateTime expiresAt { get; set; }
    }
}
