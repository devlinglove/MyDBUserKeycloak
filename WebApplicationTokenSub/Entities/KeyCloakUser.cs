namespace WebApplicationTokenSub.Entities
{
    //public class KeyCloakUser
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }

    //}


    public class KeyCloakUser
    {
        public string Sub { get; set; } = string.Empty;
        public bool EmailVerified { get; set; }
        public List<string> Roles { get; set; } = new();
        public string Name { get; set; } = string.Empty;
        public string PreferredUsername { get; set; } = string.Empty;
        public string GivenName { get; set; } = string.Empty;
        public string FamilyName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }



}
