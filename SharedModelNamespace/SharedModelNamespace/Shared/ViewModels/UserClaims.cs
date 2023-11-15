using Newtonsoft.Json;
using System.Collections.Generic;
namespace SharedModelNamespace.Shared.ViewModels
{
    public class UserClaims
    {
        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [JsonProperty("Token")]
        public string Token { get; set; }

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("Username")]
        public string Username { get; set; }

        [JsonProperty("Phone")]
        public string Phone { get; set; }

    }
}
