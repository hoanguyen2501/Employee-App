namespace EmployeeApp.Api.Helpers
{
    public class KeycloakSettings
    {
        public string AuthServerUrl { get; set; }
        public string Realm { get; set; }
        public string Resource { get; set; }
        public string Secret { get; set; }
    }
}
