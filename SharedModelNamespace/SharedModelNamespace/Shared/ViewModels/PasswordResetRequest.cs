using Newtonsoft.Json;
namespace SharedModelNamespace.Shared.ViewModels
{
    public class PasswordResetRequest
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("Password")]
        public string Password { get; set; }
    }
}
