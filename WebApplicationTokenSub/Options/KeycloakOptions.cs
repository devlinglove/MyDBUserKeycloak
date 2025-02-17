namespace WebApplicationTokenSub.Options
{
    public class KeycloakOptions
    {
        public string AdminClientId { get; set; } = string.Empty;
        public string AdminClientSecret { get; set; } = string.Empty;
        public string TokenUrl { get; set; } = string.Empty;
    }
}
