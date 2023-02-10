namespace BlogAPI.Controllers
{
    public class JWTSettings
    {
        public string Secret { get; set; }
        public TimeSpan TokenLifetime { get; set; }
    }
}
