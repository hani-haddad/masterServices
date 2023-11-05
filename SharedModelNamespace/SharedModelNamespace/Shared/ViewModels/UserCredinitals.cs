using Newtonsoft.Json;
namespace SharedModelNamespace.Shared.ViewModels
{
    public class UserCredintials
    {
        [JsonProperty("Username")]
        public string UserName { get; set; }
        [JsonProperty("Password")]
        public string Password { get; set; }
    }
}
